// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import assert from 'node:assert/strict';
import { test } from 'node:test';
import { waitForNuGet } from './wait-for-nuget.mjs';

const start = Date.parse('2026-01-01T12:00:00Z');
const json = (body, status = 200) => ({ ok: status < 400, status, json: async () => body });

function harness({ ages = {}, tag = 'v1.2.3', listedAfter = {}, apiFailures = [] }) {
    const state = { time: start, urls: [], sleeps: 0, nugetCalls: {}, logs: [], warnings: [], headers: [] };
    const fetch = async (url, options) => {
        state.urls.push(url);
        const release = /repos\/(Cratis\/\w+)\/releases\/latest/.exec(url);
        if (release) {
            state.headers.push(options?.headers);
            if (apiFailures.includes(release[1])) return json({}, 500);
            const age = ages[release[1]] ?? 24 * 60;
            return json({ tag_name: tag, published_at: new Date(start - age * 60_000).toISOString() });
        }
        const id = /flatcontainer\/([^/]+)\//.exec(url)[1];
        state.nugetCalls[id] = (state.nugetCalls[id] ?? 0) + 1;
        const after = listedAfter[id];
        const listed = after !== undefined && state.nugetCalls[id] > after;
        return json({ versions: listed ? ['1.0.0', tag.replace(/^v/, '')] : ['1.0.0'] });
    };
    const options = {
        fetch,
        sleep: async ms => { state.sleeps++; state.time += ms; },
        now: () => state.time,
        token: 'secret',
        log: message => state.logs.push(message),
        warn: message => state.warnings.push(message)
    };
    return { state, options };
}

test('does not wait when no release is recent', async () => {
    const { state, options } = harness({});
    const result = await waitForNuGet(options);
    assert.deepEqual(result, { checked: 3, awaited: 0 });
    assert.equal(state.sleeps, 0);
    assert.equal(Object.keys(state.nugetCalls).length, 0);
    assert.match(state.logs.at(-1), /Checked 3 of 3 repositories, awaited 0/);
});

test('returns without sleeping when a recent release is already listed', async () => {
    const { state, options } = harness({ ages: { 'Cratis/Arc': 5 }, listedAfter: { 'cratis.arc': 0 } });
    const result = await waitForNuGet(options);
    assert.deepEqual(result, { checked: 3, awaited: 1 });
    assert.equal(state.sleeps, 0);
    assert.deepEqual(state.warnings, []);
});

test('polls until the version becomes listed', async () => {
    const { state, options } = harness({ ages: { 'Cratis/Chronicle': 2 }, listedAfter: { 'cratis.chronicle': 3 } });
    await waitForNuGet(options);
    assert.equal(state.nugetCalls['cratis.chronicle'], 4);
    assert.equal(state.sleeps, 3);
    assert.deepEqual(state.warnings, []);
});

test('warns and succeeds on timeout', async () => {
    const { state, options } = harness({ ages: { 'Cratis/Fundamentals': 1 } });
    const result = await waitForNuGet({ ...options, intervalMs: 30_000, timeoutMs: 120_000 });
    assert.equal(result.awaited, 1);
    assert.equal(state.warnings.length, 1);
    assert.match(state.warnings[0], /^::warning::.*Cratis\.Fundamentals 1\.2\.3/);
    assert.equal(state.sleeps, 4);
});

test('skips a repository whose release lookup fails', async () => {
    const { state, options } = harness({ apiFailures: ['Cratis/Arc'], ages: { 'Cratis/Chronicle': 400 } });
    const result = await waitForNuGet(options);
    assert.deepEqual(result, { checked: 2, awaited: 0 });
    assert.equal(state.warnings.length, 1);
    assert.match(state.warnings[0], /^::warning::.*Cratis\/Arc/);
});

test('treats a thrown network error as a skip', async () => {
    const { state, options } = harness({});
    const result = await waitForNuGet({ ...options, fetch: async () => { throw new Error('offline'); } });
    assert.deepEqual(result, { checked: 0, awaited: 0 });
    assert.equal(state.warnings.length, 3);
});

test('strips the v prefix and lowercases the package id', async () => {
    const { state, options } = harness({ ages: { 'Cratis/Arc': 1 }, tag: 'v9.8.7', listedAfter: { 'cratis.arc': 0 } });
    await waitForNuGet(options);
    assert.ok(state.urls.includes('https://api.nuget.org/v3-flatcontainer/cratis.arc/index.json'));
    assert.ok(state.logs.some(line => line.includes('Cratis.Arc 9.8.7')));
});

test('sends the token as a bearer header', async () => {
    const { state, options } = harness({});
    await waitForNuGet(options);
    assert.equal(state.headers[0].Authorization, 'Bearer secret');
});

test('shares one deadline across all awaited packages', async () => {
    const { state, options } = harness({ ages: { 'Cratis/Arc': 1, 'Cratis/Chronicle': 1, 'Cratis/Fundamentals': 1 } });
    const result = await waitForNuGet({ ...options, intervalMs: 30_000, timeoutMs: 120_000 });
    assert.equal(result.awaited, 3);
    assert.equal(state.warnings.length, 3);
    assert.ok(state.time - start <= 120_000, `waited ${state.time - start} ms in total`);
});
