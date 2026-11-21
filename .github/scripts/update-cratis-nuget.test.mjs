// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import assert from 'node:assert/strict';
import { spawnSync } from 'node:child_process';
import { mkdirSync, mkdtempSync, readFileSync, rmSync, writeFileSync } from 'node:fs';
import { fileURLToPath } from 'node:url';
import { test } from 'node:test';
import { resolveVersions, update } from './update-cratis-nuget.mjs';

const chronicle = ['Cratis.Chronicle', 'Cratis.Chronicle.AspNetCore', 'Cratis.Chronicle.Aspire'];
const sample = (names = chronicle, version = '19.178.6') => `<Project>\n  <ItemGroup>\n${names.map(name => `    <PackageVersion Include="${name}" Version="${version}" />`).join('\n')}\n  </ItemGroup>\n</Project>\n`;
const inventory = () => ({
    'Cratis.Chronicle': ['19.178.1', '19.178.6', '19.178.7'],
    'Cratis.Chronicle.AspNetCore': ['19.178.1', '19.178.6', '19.178.7', '19.178.9'],
    'Cratis.Chronicle.Aspire': ['19.178.1', '19.178.6', '19.178.7', '19.178.9']
});

function registry(data = inventory()) {
    const calls = [];
    const notices = [];
    const fetch = async (url, options) => {
        assert.ok(options.signal instanceof AbortSignal);
        const match = /^https:\/\/api\.nuget\.org\/v3-flatcontainer\/(cratis(?:\.[a-z0-9.]+)?)\/index\.json$/.exec(url);
        assert.ok(match, `Unexpected registry URL: ${url}`);
        const name = Object.keys(data).find(name => name.toLowerCase() === match[1]);
        calls.push(match[1]);
        const versions = data[name];
        if (versions instanceof Error) throw versions;
        return versions ? new Response(JSON.stringify({ versions })) : new Response('', { status: 404 });
    };
    return { fetch, data, calls, notices, notice: message => notices.push(message) };
}

function assertDeferred(stub, family = 'Cratis.Chronicle') {
    assert.equal(stub.notices.length, 1);
    assert.ok(stub.notices[0].startsWith(`::warning::Deferring ${family} NuGet train:`));
    assert.match(stub.notices[0], /leaving the whole family unchanged/);
}

test('2026-11-21 inventory selects Chronicle 19.178.7 for every member, never independent 19.178.9 siblings', async () => {
    const stub = registry();
    assert.equal(await resolveVersions(sample(), stub), sample(chronicle, '19.178.7'));
    assert.deepEqual(stub.calls, chronicle.map(name => name.toLowerCase()));
    assert.match(stub.notices[0], /Updating Cratis.Chronicle NuGet train to 19.178.7/);
});

test('2026-11-21 inventory defers when siblings lack the base package common version above the floor', async () => {
    const stub = registry();
    stub.data['Cratis.Chronicle.Aspire'] = ['19.178.1', '19.178.9'];
    assert.equal(await resolveVersions(sample(), stub), sample());
    assertDeferred(stub);
    assert.match(stub.notices[0], /no common stable published version at or above every current pin/);
});

test('upgrades every member when the new train is completely published', async () => {
    const stub = registry();
    for (const versions of Object.values(stub.data)) versions.push('19.179.0');
    assert.equal(await resolveVersions(sample(), stub), sample(chronicle, '19.179.0'));
});

test('defers when there is no common newer version and the intersection is below the no-downgrade floor', async () => {
    const stub = registry();
    assert.equal(await resolveVersions(sample(chronicle, '19.178.8'), stub), sample(chronicle, '19.178.8'));
    assertDeferred(stub);
});

for (const failure of [undefined, new TypeError('fetch failed'), new DOMException('request timed out', 'TimeoutError')]) {
    test(`defers the whole family on member ${failure?.name ?? 'HTTP 404'}`, async () => {
        const stub = registry();
        stub.data['Cratis.Chronicle.Aspire'] = failure;
        assert.equal(await resolveVersions(sample(), stub), sample());
        assertDeferred(stub);
        assert.match(stub.notices[0], /Registry metadata unavailable for Cratis.Chronicle.Aspire/);
        assert.match(stub.notices[0], failure ? new RegExp(failure.message) : /HTTP 404/);
    });
}

test('defers on HTTP 500 without making partial changes', async () => {
    const stub = registry();
    stub.fetch = async () => new Response('', { status: 500 });
    assert.equal(await resolveVersions(sample(), stub), sample());
    assertDeferred(stub);
    assert.match(stub.notices[0], /HTTP 500/);
});

test('ignores prereleases and orders semver numerically rather than by strings or registry order', async () => {
    const versions = ['19.178.10', '19.178.7', '19.179.0-beta.1', '19.178.9'];
    const stub = registry(Object.fromEntries(chronicle.map(name => [name, versions])));
    assert.equal(await resolveVersions(sample(), stub), sample(chronicle, '19.178.10'));
});

test('repairs already-split pins upward to a complete train at the highest member floor', async () => {
    const stub = registry();
    stub.data['Cratis.Chronicle'].push('19.178.9');
    const original = sample(chronicle, '19.178.9').replace('Version="19.178.9"', 'Version="19.178.7"');
    assert.equal(await resolveVersions(original, stub), sample(chronicle, '19.178.9'));
});

test('never repairs split pins by downgrading a member during a publish window', async () => {
    const stub = registry();
    const original = sample().replace('Version="19.178.6"', 'Version="19.178.9"');
    assert.equal(await resolveVersions(original, stub), original);
    assertDeferred(stub);
});

test('leaves an already coherent highest common train unchanged without warning', async () => {
    const stub = registry();
    const original = sample(chronicle, '19.178.7');
    assert.equal(await resolveVersions(original, stub), original);
    assert.deepEqual(stub.notices, ['No Cratis.Chronicle NuGet train updates found (already at 19.178.7)']);
});

test('includes the Cratis metapackage in the Arc train common-version intersection', async () => {
    const names = ['Cratis', 'Cratis.Arc', 'Cratis.Arc.MongoDB'];
    const stub = registry({
        Cratis: ['23.12.12', '23.12.13'],
        'Cratis.Arc': ['23.12.12', '23.12.13', '23.12.14'],
        'Cratis.Arc.MongoDB': ['23.12.12', '23.12.13', '23.12.14']
    });
    assert.equal(await resolveVersions(sample(names, '23.12.12'), stub), sample(names, '23.12.13'));
    assert.match(stub.notices[0], /Cratis.Arc NuGet train/);
});

test('groups only dot-prefixed members of each other family and updates trains independently', async () => {
    const stub = registry({
        'Cratis.Fundamentals': ['7.34.4'],
        'Cratis.Fundamentals.SomeMember': ['7.34.4'],
        'Cratis.FundamentalsExtra': ['8.0.0'],
        'Cratis.Metrics.Roslyn': ['7.35.0'],
        ...inventory(),
        'Cratis.Chronicle.Aspire': undefined
    });
    const original = sample()
        + sample(['Cratis.Fundamentals', 'Cratis.Fundamentals.SomeMember'], '7.34.3')
        + sample(['Cratis.FundamentalsExtra'], '7.34.3')
        + sample(['Cratis.Metrics.Roslyn'], '7.34.3');
    const expected = sample()
        + sample(['Cratis.Fundamentals', 'Cratis.Fundamentals.SomeMember'], '7.34.4')
        + sample(['Cratis.FundamentalsExtra'], '8.0.0')
        + sample(['Cratis.Metrics.Roslyn'], '7.35.0');
    assert.equal(await resolveVersions(original, stub), expected);
    assert.equal(stub.notices.filter(message => message.startsWith('::warning::')).length, 1);
    assert.equal(stub.notices.length, 4);
});

test('skips and reports property-backed versions without fetching or rewriting them', async () => {
    const original = '<Project><PackageVersion Include="Cratis.Arc" Version="$(ArcVersion)" />'
        + '<PackageVersion Include="Cratis.Arc.MongoDB" Version="23.12.12" /></Project>';
    const stub = registry({ 'Cratis.Arc.MongoDB': ['23.12.13'] });
    assert.equal(await resolveVersions(original, stub), original.replace('Version="23.12.12"', 'Version="23.12.13"'));
    assert.deepEqual(stub.calls, ['cratis.arc.mongodb']);
    assert.match(stub.notices[0], /::warning::Skipping Cratis.Arc: property-backed version \$\(ArcVersion\)/);
});

test('preserves formatting, comments, CDATA, attributes, line endings, BOM and non-Cratis pins byte-for-byte', async () => {
    const original = '\uFEFF<Project>\r\n\t<!-- <PackageVersion Include="Cratis.Chronicle" Version="19.178.6" /> -->\r\n'
        + '  <![CDATA[<PackageVersion Include="Cratis.Chronicle" Version="19.178.6" />]]>\r\n'
        + '  <PackageVersion Condition="\'$(Config)\' != \'Other\'" Version = \'19.178.6\'\r\n'
        + '    Include = \'Cratis.Chronicle\' NoWarn="NU1234"/>\r\n'
        + '  <PackageVersion Include="Cratis.Chronicle.Aspire" Version="19.178.6" />\r\n'
        + '  <PackageVersion Include="Another.Package" Version="19.178.6" />\r\n</Project>';
    const expected = original.replace("Version = '19.178.6'", "Version = '19.178.7'")
        .replace('Include="Cratis.Chronicle.Aspire" Version="19.178.6"', 'Include="Cratis.Chronicle.Aspire" Version="19.178.7"');
    assert.deepEqual(Buffer.from(await resolveVersions(original, registry())), Buffer.from(expected));
});

test('does not fetch for unrelated packages or files with only property-backed Cratis pins', async () => {
    const original = sample(['NotCratis', 'CratisOther'], '1.0.0')
        + sample(['Cratis.Arc'], '$(ArcVersion)');
    const stub = registry();
    assert.equal(await resolveVersions(original, stub), original);
    assert.deepEqual(stub.calls, []);
    assert.equal(stub.notices.length, 1);
});

test('handles NuGet four-component versions numerically and stable upgrades from prerelease pins', async () => {
    const stub = registry({ 'Cratis.Chronicle': ['19.178.7', '19.178.7.10', '19.178.7.9'] });
    assert.equal(await resolveVersions(sample(['Cratis.Chronicle'], '19.178.7-preview.1'), stub), sample(['Cratis.Chronicle'], '19.178.7.10'));
});

function folder(t, original = sample()) {
    const root = fileURLToPath(new URL('../../.ai-work/692/', import.meta.url));
    mkdirSync(root, { recursive: true });
    const directory = mkdtempSync(`${root}nuget-trains-test-`);
    t.after(() => rmSync(directory, { recursive: true }));
    const propsPath = `${directory}/Directory.Packages.props`;
    writeFileSync(propsPath, original);
    return { propsPath, original, directory };
}

test('writes only selected Version attributes to the props file', async t => {
    const { propsPath } = folder(t);
    await update(propsPath, registry());
    assert.equal(readFileSync(propsPath, 'utf8'), sample(chronicle, '19.178.7'));
});

for (const scenario of ['fetch failure', 'no common version', 'no changes']) {
    test(`leaves the props bytes unchanged on ${scenario}`, async t => {
        const { propsPath, original } = folder(t, sample(chronicle, '19.178.7'));
        const stub = registry();
        if (scenario === 'fetch failure') stub.data['Cratis.Chronicle.Aspire'] = new TypeError('fetch failed');
        if (scenario === 'no common version') stub.data['Cratis.Chronicle.Aspire'] = ['19.178.9'];
        await update(propsPath, stub);
        assert.deepEqual(readFileSync(propsPath), Buffer.from(original));
    });
}

for (const response of ['not JSON', '{}', '{"versions":[42]}']) {
    test(`fails on malformed registry metadata without writing partial changes: ${response}`, async t => {
        const { propsPath, original } = folder(t);
        await assert.rejects(update(propsPath, { fetch: async () => new Response(response), notice: () => {} }));
        assert.equal(readFileSync(propsPath, 'utf8'), original);
    });
}

test('propagates programming errors without writing an earlier train selection', async t => {
    const { propsPath, original } = folder(t, sample(['Cratis.Fundamentals'], '7.34.3') + sample());
    const stub = registry({ 'Cratis.Fundamentals': ['7.34.4'] });
    const fetch = stub.fetch;
    stub.fetch = async (url, options) => url.includes('cratis.chronicle/') ? { ok: true } : fetch(url, options);
    await assert.rejects(update(propsPath, stub), TypeError);
    assert.equal(readFileSync(propsPath, 'utf8'), original);
});

test('rejects unsupported literal version ranges before querying the registry', async () => {
    const stub = registry();
    await assert.rejects(resolveVersions(sample(chronicle, '[19.178.6,20.0.0)'), stub), /Unsupported version spec/);
    assert.deepEqual(stub.calls, []);
});

const scriptPath = fileURLToPath(new URL('./update-cratis-nuget.mjs', import.meta.url));

for (const args of [[], ['missing.props', 'extra']]) {
    test(`CLI rejects invalid arguments: ${JSON.stringify(args)}`, () => {
        const result = spawnSync(process.execPath, [scriptPath, ...args], { encoding: 'utf8', timeout: 5_000 });
        assert.equal(result.status, 1);
        assert.match(result.stderr, /Usage: node .github\/scripts\/update-cratis-nuget.mjs/);
    });
}

test('CLI fails with exit 1 on an unreadable props file', t => {
    const { directory } = folder(t);
    const result = spawnSync(process.execPath, [scriptPath, `${directory}/missing.props`], { encoding: 'utf8', timeout: 5_000 });
    assert.equal(result.status, 1);
    assert.match(result.stderr, /::error::.*ENOENT/);
});

for (const scenario of ['upgrade', 'deferral', 'programming error']) {
    test(`CLI reports ${scenario} with the expected exit status and file contents`, t => {
        const { propsPath, original } = folder(t);
        const body = scenario === 'upgrade' ? { versions: ['19.178.7'] } : {};
        const setup = `globalThis.fetch = async () => new Response(${JSON.stringify(JSON.stringify(body))}, { status: ${scenario === 'deferral' ? 404 : 200} });`;
        const preload = `data:text/javascript,${encodeURIComponent(setup)}`;
        const result = spawnSync(process.execPath, ['--import', preload, scriptPath, propsPath], { encoding: 'utf8', timeout: 5_000 });
        assert.equal(result.status, scenario === 'programming error' ? 1 : 0);
        assert.match(result.stderr + result.stdout, scenario === 'upgrade' ? /Updating Cratis.Chronicle/ : scenario === 'deferral' ? /::warning::Deferring/ : /::error::Invalid registry versions/);
        assert.equal(readFileSync(propsPath, 'utf8'), scenario === 'upgrade' ? sample(chronicle, '19.178.7') : original);
    });
}
