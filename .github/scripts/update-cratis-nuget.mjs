// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { readFileSync, writeFileSync } from 'node:fs';
import { resolve } from 'node:path';
import { pathToFileURL } from 'node:url';

const stable = /^\d+\.\d+\.\d+(?:\.\d+)?$/;
const literal = /^\d+(?:\.\d+){0,3}(?:-[0-9A-Za-z.-]+)?(?:\+[0-9A-Za-z.-]+)?$/;
const compare = (left, right) => {
    const parts = version => version.split(/[-+]/, 1)[0].split('.').map(Number);
    const a = parts(left);
    const b = parts(right);
    return (a[0] ?? 0) - (b[0] ?? 0) || (a[1] ?? 0) - (b[1] ?? 0)
        || (a[2] ?? 0) - (b[2] ?? 0) || (a[3] ?? 0) - (b[3] ?? 0);
};

// A train is Cratis (the Arc metapackage) plus Cratis.Arc*, or otherwise
// the first two ID segments: the base package and its dot-prefixed members.
function train(name) {
    if (name === 'Cratis' || name.startsWith('Cratis.Arc')) return 'Cratis.Arc';
    return name.split('.').slice(0, 2).join('.');
}

function packagePin(tag) {
    if (!tag[0].startsWith('<PackageVersion')) return undefined;
    const attributes = new Map([...tag[0].matchAll(/([\w:.-]+)\s*=\s*(["'])(.*?)\2/dgs)]
        .map(attribute => [attribute[1], attribute]));
    const name = attributes.get('Include')?.[3];
    const version = attributes.get('Version');
    if (!version || !/^Cratis(?:\.|$)/.test(name)) return undefined;
    const [start, end] = version.indices[3].map(offset => tag.index + offset);
    return { name, current: version[3], start, end };
}

function collect(original, notice) {
    const groups = new Map();
    // Keep source offsets rather than serializing XML, and ignore commented-out pins.
    const tags = /<!--[\s\S]*?-->|<!\[CDATA\[[\s\S]*?\]\]>|<PackageVersion\b(?:"[^"]*"|'[^']*'|[^'">])*>/g;
    for (const tag of original.matchAll(tags)) {
        const member = packagePin(tag);
        if (!member) continue;
        const { name, current } = member;
        if (current.includes('$(')) {
            notice(`::warning::Skipping ${name}: property-backed version ${current} is not a literal pin`);
            continue;
        }
        if (!literal.test(current)) throw new Error(`Unsupported version spec ${name}: ${current}`);
        const key = train(name);
        if (!groups.has(key)) groups.set(key, []);
        groups.get(key).push(member);
    }
    return groups;
}

class Unavailable extends Error {}

async function registryIndex(name, fetch) {
    // NuGet's PackageBaseAddress/3.0.0 index exposes normalized published versions.
    // https://learn.microsoft.com/en-us/nuget/api/package-base-address-resource
    let response;
    try {
        response = await fetch(`https://api.nuget.org/v3-flatcontainer/${name.toLowerCase()}/index.json`, {
            signal: AbortSignal.timeout(60_000)
        });
    } catch (error) {
        throw new Unavailable(`Registry metadata unavailable for ${name}: ${error.message}`);
    }
    if (!response.ok) throw new Unavailable(`Registry metadata unavailable for ${name}: HTTP ${response.status}`);
    return response.json().catch(error => {
        if (error instanceof SyntaxError) throw error;
        throw new Unavailable(`Registry metadata unavailable for ${name}: ${error.message}`);
    });
}

async function published(name, fetch) {
    const index = await registryIndex(name, fetch);
    if (!Array.isArray(index?.versions) || index.versions.some(version => typeof version !== 'string')) {
        throw new Error(`Invalid registry versions for ${name}`);
    }
    return new Set(index.versions.filter(version => stable.test(version)));
}

async function selectVersion(members, fetch) {
    const versions = new Map();
    for (const { name } of members) {
        if (!versions.has(name)) versions.set(name, await published(name, fetch));
    }
    // The common-version intersection must respect every member's no-downgrade floor.
    return [...versions.get(members[0].name)]
        .filter(version => members.every(member => compare(version, member.current) >= 0 && versions.get(member.name).has(version)))
        .sort(compare).at(-1);
}

async function resolveTrain(key, members, fetch, notice) {
    let selected;
    try {
        selected = await selectVersion(members, fetch);
    } catch (error) {
        if (!(error instanceof Unavailable)) throw error;
        notice(`::warning::Deferring ${key} NuGet train: ${error.message}; leaving the whole family unchanged`);
        return [];
    }
    if (!selected) {
        notice(`::warning::Deferring ${key} NuGet train: no common stable published version at or above every current pin; leaving the whole family unchanged`);
        return [];
    }
    const changes = members.filter(member => member.current !== selected).map(member => ({ ...member, selected }));
    notice(changes.length ? `Updating ${key} NuGet train to ${selected} (${changes.length} pins)` : `No ${key} NuGet train updates found (already at ${selected})`);
    return changes;
}

export async function resolveVersions(original, { fetch = globalThis.fetch, notice = console.log } = {}) {
    const changes = [];
    for (const [key, members] of collect(original, notice)) {
        changes.push(...await resolveTrain(key, members, fetch, notice));
    }
    let updated = original;
    for (const { start, end, selected } of changes.sort((left, right) => right.start - left.start)) {
        updated = updated.slice(0, start) + selected + updated.slice(end);
    }
    return updated;
}

export async function update(propsPath, options = {}) {
    const original = readFileSync(propsPath, 'utf8');
    const updated = await resolveVersions(original, options);
    if (updated !== original) writeFileSync(propsPath, updated);
}

if (process.argv[1] && import.meta.url === pathToFileURL(resolve(process.argv[1])).href) {
    const [propsPath, ...extra] = process.argv.slice(2);
    if (!propsPath || extra.length) {
        console.error('Usage: node .github/scripts/update-cratis-nuget.mjs <Directory.Packages.props>');
        process.exitCode = 1;
    } else {
        try {
            await update(propsPath);
        } catch (error) {
            console.error(`::error::${error.message}`);
            process.exitCode = 1;
        }
    }
}
