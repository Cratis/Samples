// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { execFileSync } from 'node:child_process';
import { existsSync, readFileSync, writeFileSync } from 'node:fs';
import { resolve } from 'node:path';
import { pathToFileURL } from 'node:url';

const sections = ['dependencies', 'devDependencies'];
const stable = /^\d+\.\d+\.\d+$/;
const compare = (left, right) => {
    const a = left.split('.').map(Number);
    const b = right.split('.').map(Number);
    return a[0] - b[0] || a[1] - b[1] || a[2] - b[2];
};

// Train identity is the publisher, not the current major (unrelated trains can share a major).
function train(name) {
    if (name === '@cratis/arc' || name.startsWith('@cratis/arc.') || name === '@cratis/eslint-plugin-arc') return 'arc';
    if (name === '@cratis/components' || name === '@cratis/eslint-plugin-components') return 'components';
    if (name === '@cratis/fundamentals' || name === '@cratis/eslint-config') return 'fundamentals';
    return name;
}

class Unavailable extends Error {}

function isNotPublished(error) {
    if (typeof error.code === 'string') return ['E404', 'ETARGET'].includes(error.code);
    const diagnostics = `${error.message}\n${error.stderr ?? ''}\n${error.stdout ?? ''}`;
    return /\b(?:E404|ETARGET)\b|No match found for version|No matching version found/i.test(diagnostics);
}

function runNpm(args, cwd) {
    return execFileSync('npm', args, { cwd, encoding: 'utf8', timeout: 60_000, maxBuffer: 10 * 1024 * 1024 });
}

export function resolveVersions(manifest, { npm = runNpm, capMajors = false } = {}) {
    const entries = sections.flatMap(section => Object.entries(manifest[section] ?? {})
        .filter(([name]) => name.startsWith('@cratis/'))
        .map(([name, spec]) => ({ section, name, spec })));
    if (!entries.length) return undefined;
    const groups = new Map();
    const versions = new Map();
    const metadata = new Map();
    const ranges = new Map();
    function view(spec, field) {
        let response;
        try {
            response = npm(['view', spec, ...(field ? [field] : []), '--json']);
        } catch (error) {
            if (!isNotPublished(error)) throw error;
            throw new Unavailable(`Registry metadata unavailable for ${spec}: ${error.message}`);
        }
        // Invalid registry output is a failure, not evidence of a publish window.
        return JSON.parse(response);
    }
    function published(name) {
        if (!versions.has(name)) {
            const result = view(name, 'versions');
            const publishedVersions = Array.isArray(result) ? result : [result];
            if (publishedVersions.some(version => typeof version !== 'string')) throw new Error(`Invalid registry versions for ${name}`);
            versions.set(name, publishedVersions.filter(version => stable.test(version)).sort(compare).reverse());
        }
        return versions.get(name);
    }
    function accepts(name, version, range) {
        if (stable.test(range)) return version === range;
        const key = `${name}@${range}`;
        if (!ranges.has(key)) {
            // Let npm interpret semver/peer ranges instead of maintaining a second
            // range parser. This also checks that the matching versions are visible.
            const result = view(key, 'version');
            const matchingVersions = Array.isArray(result) ? result : [result];
            if (matchingVersions.some(version => typeof version !== 'string')) throw new Error(`Invalid registry versions for ${key}`);
            ranges.set(key, matchingVersions);
        }
        return ranges.get(key).includes(version);
    }
    function details(name, version) {
        const key = `${name}@${version}`;
        if (!metadata.has(key)) {
            const result = view(key);
            const manifest = Array.isArray(result) ? result.find(item => item?.version === version) : result;
            if (!manifest || manifest.version !== version) throw new Error(`Invalid registry metadata for ${key}`);
            metadata.set(key, manifest);
        }
        return metadata.get(key);
    }
    for (const entry of entries) {
        const parsed = /^(\^|~|=)?(\d+\.\d+\.\d+)$/.exec(entry.spec);
        if (!parsed && entry.spec !== 'latest') throw new Unavailable(`Unsupported version spec ${entry.name}: ${entry.spec}; leaving the entire npm set unchanged`);
        entry.prefix = parsed?.[1] ?? '';
        entry.current = parsed?.[2];
        entry.major = entry.current?.split('.')[0];
        const key = train(entry.name);
        if (!groups.has(key)) groups.set(key, []);
        groups.get(key).push(entry);
    }
    const candidates = [...groups].map(([key, members]) => {
        // Components upgrades need a reviewed migration; Capstone caps every train.
        const caps = members.filter(member => (capMajors || key === 'components') && member.major !== undefined).map(member => member.major);
        // Repair mixed pins upward, never downgrade any declared train member
        // (including lower-priority trains used to satisfy another train's peers).
        const floor = members.map(member => member.current).filter(Boolean).sort(compare).at(-1);
        const common = published(members[0].name).filter(version => (!floor || compare(version, floor) >= 0)
            && caps.every(major => version.split('.')[0] === major)
            && members.every(member => published(member.name).includes(version)));
        return { key, members, common };
    });
    const chosen = new Map();
    const trainVersions = new Map();
    function coherent() {
        const required = new Map(chosen);
        const requiredTrains = new Map(trainVersions);
        const checked = new Set();
        function check(name) {
            if (checked.has(name)) return true;
            checked.add(name);
            const info = details(name, required.get(name));
            // Optional dependencies/peers do not have to be installed, but a directly
            // declared optional peer must still agree with the selected version.
            const dependencies = [
                ...Object.entries(info.dependencies ?? {}),
                ...Object.entries(info.peerDependencies ?? {}),
                ...Object.entries(info.optionalDependencies ?? {}).filter(([dependency]) => required.has(dependency))
            ];
            for (const [dependency, range] of dependencies) {
                if (!dependency.startsWith('@cratis/')) continue;
                if (!required.has(dependency)) {
                    if (!info.dependencies?.[dependency] && info.peerDependenciesMeta?.[dependency]?.optional) continue;
                    const target = requiredTrains.get(train(dependency));
                    const available = published(dependency);
                    const version = target ?? available.find(candidate => accepts(dependency, candidate, range));
                    if (!version || !available.includes(version)) return false;
                    required.set(dependency, version);
                    requiredTrains.set(train(dependency), version);
                }
                if (!accepts(dependency, required.get(dependency), range) || !check(dependency)) return false;
            }
            return true;
        }
        return [...chosen.keys()].every(check);
    }
    function compatibleSelections() {
        // Prune incompatible majors before enumerating every older combination
        // of the remaining trains. Exact metadata is cached across attempts.
        for (const [name, version] of chosen) {
            const info = details(name, version);
            const constraints = [...Object.entries(info.dependencies ?? {}), ...Object.entries(info.peerDependencies ?? {})];
            if (constraints.some(([dependency, range]) => chosen.has(dependency) && !accepts(dependency, chosen.get(dependency), range))) return false;
        }
        return true;
    }
    function select(index) {
        if (index === candidates.length) return coherent();
        const { key, members, common } = candidates[index];
        for (const version of common) {
            trainVersions.set(key, version);
            for (const member of members) chosen.set(member.name, version);
            try {
                if (compatibleSelections() && select(index + 1)) return true;
            } catch (error) {
                // An exact manifest may still be invisible after the versions index
                // appeared. Try an older complete train, not independent latest pins.
                if (!(error instanceof Unavailable)) throw error;
            }
        }
        trainVersions.delete(key);
        for (const member of members) chosen.delete(member.name);
        return false;
    }
    if (!select(0)) return undefined;
    const updated = structuredClone(manifest);
    for (const { section, name, prefix } of entries) updated[section][name] = `${prefix}${chosen.get(name)}`;
    return updated;
}

export function update(folder, { npm = runNpm, withLockfile = false, notice = console.log } = {}) {
    const manifestPath = resolve(folder, 'package.json');
    const lockPath = resolve(folder, 'package-lock.json');
    const original = readFileSync(manifestPath, 'utf8');
    const manifest = JSON.parse(original);
    let updated;
    try {
        updated = resolveVersions(manifest, { npm, capMajors: withLockfile });
    } catch (error) {
        if (!(error instanceof Unavailable)) throw error;
        notice(`::notice::Skipping npm update in ${folder}: ${error.message}`);
        return;
    }
    if (!updated) {
        notice(`::notice::Skipping npm update in ${folder}: no complete, mutually compatible published @cratis/* release trains are available yet`);
        return;
    }
    if (JSON.stringify(updated) === JSON.stringify(manifest)) {
        notice(`No @cratis/* train updates found in ${folder}`);
        return;
    }
    const lock = withLockfile ? readFileSync(lockPath) : undefined;
    const formatted = `${JSON.stringify(updated, null, 2)}\n`;
    writeFileSync(manifestPath, formatted);
    if (withLockfile) {
        try {
            // Ranges must not let npm independently pick newer, partially published
            // trains. Install exact selections, then restore the declared prefixes.
            const exact = structuredClone(updated);
            for (const section of sections) {
                for (const name of Object.keys(exact[section] ?? {})) {
                    if (name.startsWith('@cratis/')) exact[section][name] = exact[section][name].replace(/^[\^~=]/, '');
                }
            }
            writeFileSync(manifestPath, `${JSON.stringify(exact, null, 2)}\n`);
            npm(['install', '--package-lock-only', '--ignore-scripts', '--no-audit', '--no-fund'], folder);
            const refreshed = JSON.parse(readFileSync(lockPath, 'utf8'));
            const root = refreshed.packages?.[''];
            if (!root) throw new Error('Refreshed npm lockfile has no root package');
            for (const section of sections) {
                for (const [name, spec] of Object.entries(updated[section] ?? {})) {
                    if (!name.startsWith('@cratis/')) continue;
                    // Check what npm actually locked, rather than assuming it used
                    // the exact manifest for a package already in the old lockfile.
                    if (refreshed.packages[`node_modules/${name}`]?.version !== exact[section][name]) {
                        throw new Unavailable(`Lockfile did not resolve the selected published version of ${name}`);
                    }
                    root[section][name] = spec;
                }
            }
            writeFileSync(lockPath, `${JSON.stringify(refreshed, null, 2)}\n`);
            writeFileSync(manifestPath, formatted);
        } catch (error) {
            // A registry visibility race can still occur between resolution and
            // installation. Never leave a manifest/lockfile pair partially updated.
            writeFileSync(manifestPath, original);
            writeFileSync(lockPath, lock);
            if (!(error instanceof Unavailable) && !isNotPublished(error)) throw error;
            notice(`::notice::Skipping npm update in ${folder}: registry publication is still settling; restored package.json and package-lock.json`);
            return;
        }
    }
    notice(`Updated coherent @cratis/* release trains in ${folder}`);
}

if (process.argv[1] && import.meta.url === pathToFileURL(resolve(process.argv[1])).href) {
    const [folder, option] = process.argv.slice(2);
    if (!folder || (option && option !== '--with-lockfile') || !existsSync(resolve(folder, 'package.json'))) {
        console.error('Usage: node .github/scripts/update-cratis-npm.mjs <folder> [--with-lockfile]');
        process.exitCode = 1;
    } else {
        update(folder, { withLockfile: option === '--with-lockfile' });
    }
}
