// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import assert from 'node:assert/strict';
import { spawnSync } from 'node:child_process';
import { mkdtempSync, mkdirSync, readFileSync, readdirSync, rmSync, writeFileSync } from 'node:fs';
import { tmpdir } from 'node:os';
import { dirname, join } from 'node:path';
import { fileURLToPath } from 'node:url';
import { test } from 'node:test';

const script = join(dirname(fileURLToPath(import.meta.url)), 'run-specs.sh');
const runnerArguments = ['--project', 'Safe.Specs.csproj', '--configuration', 'Debug', '--no-build', '--no-restore', '--', '-class', 'Safe.Specs.*'];

function runWrapper(t, report, nativeExit = 0) {
    const directory = mkdtempSync(join(tmpdir(), 'run-specs-test-'));
    t.after(() => rmSync(directory, { recursive: true, force: true }));
    const temporaryReports = join(directory, 'reports');
    mkdirSync(temporaryReports);
    const argumentsFile = join(directory, 'arguments.json');
    writeFileSync(join(directory, 'dotnet'), `#!/usr/bin/env node
const fs = require('node:fs');
const args = process.argv.slice(2);
fs.writeFileSync(process.env.ARGUMENTS_FILE, JSON.stringify(args));
if (process.env.REPORT_XML !== undefined) {
    fs.writeFileSync(args[args.indexOf('-result-xml') + 1], process.env.REPORT_XML);
}
process.exit(Number(process.env.NATIVE_EXIT));
`, { mode: 0o755 });
    const env = {
        ...process.env,
        PATH: `${directory}:${process.env.PATH}`,
        TMPDIR: temporaryReports,
        ARGUMENTS_FILE: argumentsFile,
        NATIVE_EXIT: String(nativeExit)
    };
    delete env.REPORT_XML;
    if (report !== undefined) env.REPORT_XML = report;
    const result = spawnSync('bash', [script, ...runnerArguments], { env, encoding: 'utf8' });
    assert.equal(result.error, undefined);
    assert.equal(result.signal, null);
    assert.deepEqual(readdirSync(temporaryReports), []);
    return { ...result, arguments: JSON.parse(readFileSync(argumentsFile, 'utf8')) };
}

test('accepts a positive sum across assemblies and forwards runner arguments', t => {
    const result = runWrapper(t, '<assemblies><assembly total="0"/><assembly total="4"/></assemblies>');
    assert.equal(result.status, 0, result.stderr);
    assert.deepEqual(result.arguments.slice(0, -2), ['run', ...runnerArguments]);
    assert.equal(result.arguments.at(-2), '-result-xml');
});

test('preserves native failure before attempting to parse a missing report', t => {
    const result = runWrapper(t, undefined, 42);
    assert.equal(result.status, 42);
    assert.equal(result.stderr, '');
});

for (const [name, report] of [
    ['missing report', undefined],
    ['malformed XML', '<assemblies><assembly total="4"></assemblies>'],
    ['wrong root', '<results><assembly total="4"/></results>'],
    ['missing assemblies', '<assemblies/>'],
    ['unexpected child', '<assemblies><other total="4"/></assemblies>'],
    ['missing total', '<assemblies><assembly/></assemblies>'],
    ['negative total', '<assemblies><assembly total="-1"/></assemblies>'],
    ['fractional total', '<assemblies><assembly total="1.5"/></assemblies>'],
    ['nonnumeric total', '<assemblies><assembly total="four"/></assemblies>'],
    ['zero total', '<assemblies><assembly total="0"/><assembly total="0"/></assemblies>']
]) {
    test(`rejects ${name}`, t => {
        const result = runWrapper(t, report);
        assert.equal(result.status, 1);
        assert.match(result.stderr, /Invalid xUnit report/);
    });
}
