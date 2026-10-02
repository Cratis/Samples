// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import assert from 'node:assert/strict';
import { mkdirSync, mkdtempSync, readFileSync, rmSync, writeFileSync } from 'node:fs';
import { fileURLToPath } from 'node:url';
import { test } from 'node:test';
import { resolveVersions, update } from './update-cratis-npm.mjs';

const fixture = JSON.parse(readFileSync(new URL('./fixtures/cratis-npm-registry.json', import.meta.url)));
const sample = () => ({
    name: 'sample',
    dependencies: {
        '@cratis/arc': '^22.38.0',
        '@cratis/arc.react': '~22.39.0',
        '@cratis/arc.vite': '22.39.0',
        '@cratis/components': '^4.23.0',
        '@cratis/fundamentals': '^7.20.0',
        react: '^19.0.0'
    },
    devDependencies: {
        '@cratis/eslint-plugin-arc': '^22.38.0',
        '@cratis/eslint-plugin-components': '~4.23.0',
        '@cratis/eslint-config': '7.20.0',
        typescript: '6.0.0'
    }
});

function registry(data = structuredClone(fixture)) {
    const calls = [];
    const npm = args => {
        calls.push(args);
        assert.equal(args[0], 'view');
        assert.equal(args.at(-1), '--json');
        if (args[2] === 'versions') {
            if (!data[args[1]]) throw Object.assign(new Error('package not yet published'), { code: 'E404' });
            return JSON.stringify(Object.keys(data[args[1]]));
        }
        if (args[2] === 'version') {
            assert.ok(data.ranges[args[1]], `Missing npm range fixture: ${args[1]}`);
            const name = args[1].slice(0, args[1].lastIndexOf('@'));
            return JSON.stringify(data.ranges[args[1]].filter(version => data[name]?.[version]));
        }
        const separator = args[1].lastIndexOf('@');
        const name = args[1].slice(0, separator);
        const version = args[1].slice(separator + 1);
        const info = data[name]?.[version];
        if (!info || info.unavailable) throw Object.assign(new Error('manifest not yet visible'), { code: 'E404' });
        return JSON.stringify({ name, version, ...info });
    };
    return { npm, data, calls };
}

function assertArc(manifest, version) {
    assert.equal(manifest.dependencies['@cratis/arc'], `^${version}`);
    assert.equal(manifest.dependencies['@cratis/arc.react'], `~${version}`);
    assert.equal(manifest.dependencies['@cratis/arc.vite'], version);
    assert.equal(manifest.devDependencies['@cratis/eslint-plugin-arc'], `^${version}`);
}

test('selects the newest complete trains, preserves prefixes and non-Cratis dependencies', () => {
    const manifest = sample();
    const before = structuredClone(manifest);
    const result = resolveVersions(manifest, { ...registry(), capMajors: true });
    assertArc(result, '22.40.0');
    assert.equal(result.dependencies['@cratis/components'], '^4.23.1');
    assert.equal(result.devDependencies['@cratis/eslint-plugin-components'], '~4.23.1');
    assert.equal(result.dependencies['@cratis/fundamentals'], '^7.22.0');
    assert.equal(result.devDependencies['@cratis/eslint-config'], '7.22.0');
    assert.equal(result.dependencies.react, '^19.0.0');
    assert.equal(result.devDependencies.typescript, '6.0.0');
    assert.deepEqual(manifest, before);
});

test('skips a mid-publish intersection below the highest declared train member', () => {
    const stub = registry();
    delete stub.data['@cratis/arc']['22.40.0'];
    delete stub.data['@cratis/arc.vite']['22.39.0'];
    assert.equal(resolveVersions(sample(), { ...stub, capMajors: true }), undefined);
});

test('repairs mixed pins upward to a complete train at the highest declared floor', () => {
    const manifest = sample();
    manifest.devDependencies['@cratis/eslint-plugin-arc'] = '^22.40.0';
    assertArc(resolveVersions(manifest, { ...registry(), capMajors: true }), '22.40.0');
});

test('does not downgrade Components to keep a newer Arc train', () => {
    const stub = registry();
    const manifest = sample();
    manifest.dependencies['@cratis/components'] = '^4.23.1';
    stub.data['@cratis/components']['4.23.1'].peerDependencies['@cratis/arc'] = '<22.40.0';
    stub.data.ranges['@cratis/arc@<22.40.0'] = ['22.38.0', '22.39.0'];
    const result = resolveVersions(manifest, stub);
    assertArc(result, '22.39.0');
    assert.equal(result.dependencies['@cratis/components'], '^4.23.1');
    manifest.dependencies['@cratis/arc'] = '^22.40.0';
    assert.equal(resolveVersions(manifest, stub), undefined);
});

test('does not downgrade Fundamentals to keep a newer Arc train', () => {
    const stub = registry();
    const manifest = sample();
    manifest.dependencies['@cratis/fundamentals'] = '^7.22.0';
    stub.data['@cratis/arc.react']['22.40.0'].dependencies['@cratis/fundamentals'] = '^7.20.0 <7.22.0';
    stub.data.ranges['@cratis/fundamentals@^7.20.0 <7.22.0'] = ['7.20.0'];
    const result = resolveVersions(manifest, { ...stub, capMajors: true });
    assertArc(result, '22.39.0');
    assert.equal(result.dependencies['@cratis/fundamentals'], '^7.22.0');
    manifest.dependencies['@cratis/arc'] = '^22.40.0';
    assert.equal(resolveVersions(manifest, { ...stub, capMajors: true }), undefined);
});

test('repairs drift and moves dependencies and devDependencies together', () => {
    const stub = registry();
    delete stub.data['@cratis/eslint-plugin-arc']['22.40.0'];
    assertArc(resolveVersions(sample(), { ...stub, capMajors: true }), '22.39.0');
});

test('falls back when a versions index is visible before an exact manifest', () => {
    const stub = registry();
    stub.data['@cratis/arc']['22.40.0'].unavailable = true;
    assertArc(resolveVersions(sample(), { ...stub, capMajors: true }), '22.39.0');
});

test('checks the published exact cross-pins, not just equal version numbers', () => {
    const stub = registry();
    stub.data['@cratis/arc.vite']['22.40.0'].dependencies['@cratis/arc'] = '22.39.0';
    assertArc(resolveVersions(sample(), { ...stub, capMajors: true }), '22.39.0');
});

test('checks required transitive train members even when not directly declared', () => {
    const stub = registry();
    const manifest = { dependencies: { '@cratis/arc.vite': '22.38.0' } };
    delete stub.data['@cratis/arc']['23.0.0'];
    delete stub.data['@cratis/arc']['22.40.0'];
    assert.equal(resolveVersions(manifest, stub).dependencies['@cratis/arc.vite'], '22.39.0');
});

test('checks peers across trains and falls back instead of creating incompatible majors', () => {
    assertArc(resolveVersions(sample(), registry()), '22.40.0');
});

test('does not cap other workspace trains when their peers permit a major upgrade', () => {
    const manifest = sample();
    delete manifest.dependencies['@cratis/components'];
    delete manifest.devDependencies['@cratis/eslint-plugin-components'];
    const result = resolveVersions(manifest, registry());
    assertArc(result, '23.0.0');
    assert.equal(result.dependencies['@cratis/fundamentals'], '^8.0.0');
});

test('Components and its eslint plugin stay on the existing major without a migration', () => {
    const stub = registry();
    delete stub.data['@cratis/eslint-plugin-components']['4.23.1'];
    const result = resolveVersions(sample(), stub);
    assert.equal(result.dependencies['@cratis/components'], '^4.23.0');
    assert.equal(result.devDependencies['@cratis/eslint-plugin-components'], '~4.23.0');
});

test('different publishers sharing a major are not treated as the same train', () => {
    const stub = registry();
    stub.data['@cratis/unrelated'] = { '22.5.0': {} };
    const manifest = sample();
    manifest.dependencies['@cratis/unrelated'] = '22.1.0';
    const result = resolveVersions(manifest, { ...stub, capMajors: true });
    assertArc(result, '22.40.0');
    assert.equal(result.dependencies['@cratis/unrelated'], '22.5.0');
});

test('ignores prereleases and sorts versions numerically', () => {
    const stub = registry();
    stub.data['@cratis/unrelated'] = { '1.9.0': {}, '1.10.0': {}, '2.0.0-beta.1': {} };
    assert.equal(resolveVersions({ dependencies: { '@cratis/unrelated': '~1.0.0' } }, stub).dependencies['@cratis/unrelated'], '~1.10.0');
});

test('skips the whole npm set when a train has no shared published version', () => {
    const stub = registry();
    stub.data['@cratis/arc'] = { '22.37.0': {} };
    assert.equal(resolveVersions(sample(), { ...stub, capMajors: true }), undefined);
});

test('handles devDependencies-only manifests and packages declared in both sections', () => {
    const stub = registry();
    const manifest = { devDependencies: { '@cratis/arc': '^22.38.0' } };
    assert.equal(resolveVersions(manifest, { ...stub, capMajors: true }).devDependencies['@cratis/arc'], '^22.40.0');
    manifest.dependencies = { '@cratis/arc': '~22.39.0' };
    const result = resolveVersions(manifest, { ...stub, capMajors: true });
    assert.equal(result.dependencies['@cratis/arc'], '~22.40.0');
    assert.equal(result.devDependencies['@cratis/arc'], '^22.40.0');
});

test('supports latest without a numeric major cap and leaves empty manifests alone', () => {
    assert.equal(resolveVersions({ dependencies: { '@cratis/arc': 'latest' } }, { ...registry(), capMajors: true }).dependencies['@cratis/arc'], '23.0.0');
    assert.equal(resolveVersions({ dependencies: { react: '19.0.0' } }, registry()), undefined);
});

for (const project of ['Arc/React', 'Library/Lending', 'Library/Members', 'Capstone']) {
    test(`does not downgrade the actual ${project} manifest to older registry fixtures`, () => {
        const manifest = JSON.parse(readFileSync(new URL(`../../${project}/package.json`, import.meta.url)));
        const before = structuredClone(manifest);
        assert.equal(resolveVersions(manifest, { ...registry(), capMajors: project === 'Capstone' }), undefined);
        assert.deepEqual(manifest, before);
    });
}

test('accepts npm metadata as a singleton array and a scalar published version', () => {
    const stub = registry();
    const manifest = { dependencies: { '@cratis/arc': '22.38.0' } };
    const result = resolveVersions(manifest, { npm: args => {
        if (args[2] === 'versions') return JSON.stringify('22.39.0');
        return JSON.stringify([JSON.parse(stub.npm(args))]);
    } });
    assert.equal(result.dependencies['@cratis/arc'], '22.39.0');
});

test('checks required dependencies even when a peer with the same name is optional', () => {
    const stub = registry();
    const manifest = { dependencies: { '@cratis/arc.vite': '22.38.0' } };
    for (const [version, info] of Object.entries(stub.data['@cratis/arc.vite'])) {
        if (version === '22.38.0') continue;
        info.peerDependencies = { '@cratis/arc': version };
        info.peerDependenciesMeta = { '@cratis/arc': { optional: true } };
        delete stub.data['@cratis/arc'][version];
    }
    assert.equal(resolveVersions(manifest, stub).dependencies['@cratis/arc.vite'], '22.38.0');
});

test('asks npm to interpret peer ranges and caches matching published versions', () => {
    const stub = registry();
    resolveVersions(sample(), stub);
    assert.equal(stub.calls.filter(args => args[1] === '@cratis/arc@>=20.3.1 <23').length, 1);
    assert.equal(stub.calls.filter(args => args[1] === '@cratis/fundamentals@^7.10.3').length, 1);
});

function folder(t) {
    const root = fileURLToPath(new URL('../../.ai-work/', import.meta.url));
    mkdirSync(root, { recursive: true });
    const directory = mkdtempSync(`${root}npm-trains-test-`);
    t.after(() => rmSync(directory, { recursive: true }));
    const original = `${JSON.stringify(sample(), null, 4)}\n`;
    const lock = '{"lockfileVersion":3,"fixture":"original"}\n';
    writeFileSync(`${directory}/package.json`, original);
    writeFileSync(`${directory}/package-lock.json`, lock);
    return { directory, original, lock };
}

test('refreshes the lockfile once, after resolving every package, with exact manifest specs', t => {
    const { directory } = folder(t);
    const stub = registry();
    let installs = 0;
    update(directory, { withLockfile: true, notice: () => {}, npm: (args, cwd) => {
        if (args[0] === 'view') return stub.npm(args);
        installs++;
        assert.equal(cwd, directory);
        assert.deepEqual(args, ['install', '--package-lock-only', '--ignore-scripts', '--no-audit', '--no-fund']);
        const exact = JSON.parse(readFileSync(`${directory}/package.json`));
        assert.equal(exact.dependencies['@cratis/arc'], '22.40.0');
        assert.equal(exact.dependencies['@cratis/arc.react'], '22.40.0');
        assert.equal(exact.devDependencies['@cratis/eslint-plugin-arc'], '22.40.0');
        const packages = { '': exact };
        for (const section of ['dependencies', 'devDependencies']) {
            for (const [name, version] of Object.entries(exact[section])) {
                if (name.startsWith('@cratis/')) packages[`node_modules/${name}`] = { version };
            }
        }
        writeFileSync(`${directory}/package-lock.json`, JSON.stringify({ fixture: 'updated', packages }));
    } });
    assert.equal(installs, 1);
    const refreshed = JSON.parse(readFileSync(`${directory}/package-lock.json`));
    assert.equal(refreshed.fixture, 'updated');
    assertArc(refreshed.packages[''], '22.40.0');
    assertArc(JSON.parse(readFileSync(`${directory}/package.json`)), '22.40.0');
    assert.equal(refreshed.packages['node_modules/@cratis/arc'].version, '22.40.0');
});

test('rejects a lockfile that did not use the selected exact version and restores both files', t => {
    const { directory, original, lock } = folder(t);
    const stub = registry();
    const notices = [];
    update(directory, { withLockfile: true, notice: message => notices.push(message), npm: args => {
        if (args[0] === 'view') return stub.npm(args);
        writeFileSync(`${directory}/package-lock.json`, JSON.stringify({ packages: {
            '': sample(), 'node_modules/@cratis/arc': { version: '22.39.0' }
        } }));
    } });
    assert.equal(readFileSync(`${directory}/package.json`, 'utf8'), original);
    assert.equal(readFileSync(`${directory}/package-lock.json`, 'utf8'), lock);
    assert.match(notices[0], /::notice::.*restored/);
});

for (const code of ['ETARGET', 'E404', 'ERESOLVE', 'ETIMEDOUT', 'ECONNRESET', 'EAI_AGAIN', 'ENOENT', 'E401', 'E403']) {
    test(`restores both files on ${code}; only registry publication failures are skipped`, t => {
        const { directory, original, lock } = folder(t);
        const stub = registry();
        const notices = [];
        const perform = () => update(directory, { withLockfile: true, notice: message => notices.push(message), npm: args => {
            if (args[0] === 'view') return stub.npm(args);
            writeFileSync(`${directory}/package-lock.json`, 'partial lockfile');
            const error = new Error('npm install failed');
            error.stderr = `npm error code ${code}`;
            throw error;
        } });
        if (!['ETARGET', 'E404'].includes(code)) assert.throws(perform, /npm install failed/);
        else {
            assert.doesNotThrow(perform);
            assert.match(notices[0], /::notice::.*publication.*restored/);
        }
        assert.equal(readFileSync(`${directory}/package.json`, 'utf8'), original);
        assert.equal(readFileSync(`${directory}/package-lock.json`, 'utf8'), lock);
    });
}

for (const scenario of ['missing train', 'only older trains', 'unsupported spec', 'no changes']) {
    test(`leaves files byte-for-byte unchanged and never installs: ${scenario}`, t => {
        const { directory, lock } = folder(t);
        const stub = registry();
        if (scenario === 'missing train') stub.data['@cratis/arc'] = {};
        if (scenario === 'only older trains') {
            const manifest = sample();
            manifest.dependencies['@cratis/arc.vite'] = '22.41.0';
            writeFileSync(`${directory}/package.json`, JSON.stringify(manifest));
        }
        if (scenario === 'unsupported spec') {
            const manifest = sample();
            manifest.dependencies['@cratis/arc'] = 'workspace:*';
            writeFileSync(`${directory}/package.json`, JSON.stringify(manifest));
        }
        if (scenario === 'no changes') {
            writeFileSync(`${directory}/package.json`, JSON.stringify(resolveVersions(sample(), { ...stub, capMajors: true })));
        }
        const original = readFileSync(`${directory}/package.json`, 'utf8');
        const notices = [];
        update(directory, { withLockfile: true, notice: message => notices.push(message), npm: args => {
            assert.equal(args[0], 'view');
            return stub.npm(args);
        } });
        assert.equal(readFileSync(`${directory}/package.json`, 'utf8'), original);
        assert.equal(readFileSync(`${directory}/package-lock.json`, 'utf8'), lock);
        assert.match(notices[0], scenario === 'no changes' ? /No .* updates/ : /::notice::Skipping npm update/);
    });
}

for (const code of ['ETIMEDOUT', 'ECONNRESET', 'EAI_AGAIN', 'ENOENT', 'E401', 'E403', 'timeout', 'malformed JSON', 'invalid metadata']) {
    for (const stage of ['versions', 'metadata', 'peer range']) {
        test(`${code} during ${stage} fails without fallback, install, or file changes`, t => {
            const { directory, original, lock } = folder(t);
            const stub = registry();
            const error = new Error(`npm ${code}`);
            if (!['timeout', 'malformed JSON', 'invalid metadata'].includes(code)) error.code = code;
            const notices = [];
            const npm = args => {
                assert.equal(args[0], 'view');
                const matches = stage === 'versions' ? args[2] === 'versions'
                    : stage === 'metadata' ? args[1] === '@cratis/arc@22.40.0'
                    : args[1] === '@cratis/arc@>=20.3.1 <23';
                if (matches) {
                    if (code === 'malformed JSON') return 'not JSON: E404';
                    if (code === 'invalid metadata') return '{"error":"E401"}';
                    throw error;
                }
                return stub.npm(args);
            };
            assert.throws(() => update(directory, { npm, withLockfile: true, notice: message => notices.push(message) }),
                code === 'malformed JSON' ? SyntaxError : code === 'invalid metadata' ? /Invalid registry/ : thrown => thrown === error);
            assert.equal(readFileSync(`${directory}/package.json`, 'utf8'), original);
            assert.equal(readFileSync(`${directory}/package-lock.json`, 'utf8'), lock);
            assert.deepEqual(notices, []);
            assert.equal(stub.calls.some(args => args[1] === '@cratis/arc@22.39.0'), false);
        });
    }
}

for (const format of ['JSON', 'npm ERR! code', 'npm error code']) {
    for (const code of ['E404', 'ETARGET']) {
        test(`${format} ${code} permits fallback to an older complete train`, () => {
            const stub = registry();
            const error = Object.assign(new Error('Command failed: npm view'), { status: 1 });
            if (format === 'JSON') error.stdout = JSON.stringify({ error: { code, summary: 'Not published yet' } });
            else error.stderr = `${format} ${code}\n`;
            const result = resolveVersions(sample(), { capMajors: true, npm: args => {
                if (args[1] === '@cratis/arc@22.40.0') throw error;
                return stub.npm(args);
            } });
            assertArc(result, '22.39.0');
        });
    }
}

for (const message of ['No match found for version 22.40.0', 'npm ERR! No match found for version 22.40.0', 'npm error No match found for version 22.40.0']) {
    test(`a narrow missing-version message without a code permits fallback: ${message}`, () => {
        const stub = registry();
        const result = resolveVersions(sample(), { capMajors: true, npm: args => {
            if (args[1] === '@cratis/arc@22.40.0') throw Object.assign(new Error(message), { status: 1 });
            return stub.npm(args);
        } });
        assertArc(result, '22.39.0');
    });
}

for (const format of ['JSON', 'npm ERR! code', 'npm error code']) {
    for (const code of ['FETCH_ERROR', 'E401', 'E403', 'ETIMEDOUT']) {
        test(`${format} ${code} fails even when diagnostics mention E404 and ETARGET`, t => {
            const { directory, original, lock } = folder(t);
            const stub = registry();
            const message = 'Invalid JSON response: E404 / ETARGET; No match found for version 22.40.0';
            const error = Object.assign(new Error(`Command failed: npm view\n${message}`), { status: 1 });
            if (format === 'JSON') error.stdout = JSON.stringify({ error: { code, summary: message } });
            else error.stderr = `${format} ${code}\nnpm error ${message}\n`;
            const notices = [];
            assert.throws(() => update(directory, { withLockfile: true, notice: message => notices.push(message), npm: args => {
                assert.equal(args[0], 'view');
                if (args[1] === '@cratis/arc@22.40.0') throw error;
                return stub.npm(args);
            } }), thrown => thrown === error);
            assert.equal(readFileSync(`${directory}/package.json`, 'utf8'), original);
            assert.equal(readFileSync(`${directory}/package-lock.json`, 'utf8'), lock);
            assert.deepEqual(notices, []);
            assert.equal(stub.calls.some(args => args[1] === '@cratis/arc@22.39.0'), false);
        });
    }
}

for (const stderr of ['npm error code\nNo match found for version 22.40.0', 'npm ERR! code UNKNOWN\nNo match found for version 22.40.0', 'Invalid JSON response: E404 / ETARGET']) {
    test(`unknown or absent codes do not turn arbitrary error text into publication misses: ${stderr}`, () => {
        const stub = registry();
        const error = Object.assign(new Error('Command failed: npm view'), { status: 1, stderr });
        assert.throws(() => resolveVersions(sample(), { capMajors: true, npm: args => {
            if (args[1] === '@cratis/arc@22.40.0') throw error;
            return stub.npm(args);
        } }), thrown => thrown === error);
    });
}

test('workspace updates do not run npm install or touch a lockfile', t => {
    const { directory, lock } = folder(t);
    update(directory, { ...registry(), notice: () => {} });
    assertArc(JSON.parse(readFileSync(`${directory}/package.json`)), '22.40.0');
    assert.equal(readFileSync(`${directory}/package-lock.json`, 'utf8'), lock);
});
