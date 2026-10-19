// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { resolve } from 'node:path';
import { pathToFileURL } from 'node:url';

export const sources = {
    'Cratis/Arc': 'Cratis.Arc',
    'Cratis/Chronicle': 'Cratis.Chronicle',
    'Cratis/Fundamentals': 'Cratis.Fundamentals'
};

const minute = 60_000;

async function latestRelease(repository, { fetch, token }) {
    const headers = { Accept: 'application/vnd.github+json' };
    if (token) headers.Authorization = `Bearer ${token}`;
    const response = await fetch(`https://api.github.com/repos/${repository}/releases/latest`, { headers });
    if (!response.ok) throw new Error(`GitHub API returned ${response.status}`);
    const release = await response.json();
    const published = Date.parse(release.published_at);
    if (!release.tag_name || Number.isNaN(published)) throw new Error('release has no tag or publication time');
    return { version: release.tag_name.replace(/^v/, ''), published };
}

async function isListed(packageId, version, fetch) {
    const response = await fetch(`https://api.nuget.org/v3-flatcontainer/${packageId.toLowerCase()}/index.json`);
    // 404 means NuGet does not know the package yet; anything else is a transient failure, so keep polling.
    if (!response.ok) return false;
    const index = await response.json();
    return Array.isArray(index.versions) && index.versions.includes(version);
}

async function awaitListing(packageId, version, { fetch, sleep, now, intervalMs, deadline, log, warn }) {
    log(`Waiting for ${packageId} ${version} to appear on NuGet`);
    for (;;) {
        let listed = false;
        try {
            listed = await isListed(packageId, version, fetch);
        } catch (error) {
            log(`NuGet lookup for ${packageId} failed (${error.message}); will retry`);
        }
        if (listed) {
            log(`${packageId} ${version} is available on NuGet`);
            return true;
        }
        if (now() + intervalMs > deadline) {
            warn(`::warning::Timed out waiting for ${packageId} ${version} on NuGet; continuing with the latest available`);
            return false;
        }
        await sleep(intervalMs);
    }
}

export async function waitForNuGet({
    fetch = globalThis.fetch,
    sleep = ms => new Promise(done => setTimeout(done, ms)),
    now = Date.now,
    token = '',
    intervalMs = 30_000,
    timeoutMs = 20 * minute,
    recentMs = 60 * minute,
    log = console.log,
    warn = console.log
} = {}) {
    let checked = 0;
    let awaited = 0;
    // One deadline for the whole run, so several late packages cannot add up past the job timeout.
    const deadline = now() + timeoutMs;
    for (const [repository, packageId] of Object.entries(sources)) {
        let release;
        try {
            release = await latestRelease(repository, { fetch, token });
        } catch (error) {
            warn(`::warning::Could not read the latest release of ${repository} (${error.message}); skipping`);
            continue;
        }
        checked++;
        if (now() - release.published >= recentMs) {
            log(`${repository} ${release.version} was released over ${recentMs / minute} minutes ago; no wait needed`);
            continue;
        }
        awaited++;
        await awaitListing(packageId, release.version, { fetch, sleep, now, intervalMs, deadline, log, warn });
    }
    log(`Checked ${checked} of ${Object.keys(sources).length} repositories, awaited ${awaited}`);
    return { checked, awaited };
}

if (process.argv[1] && import.meta.url === pathToFileURL(resolve(process.argv[1])).href) {
    const seconds = (name, fallback) => (Number(process.env[name]) > 0 ? Number(process.env[name]) * 1000 : fallback);
    await waitForNuGet({
        token: process.env.GITHUB_TOKEN ?? '',
        intervalMs: seconds('NUGET_POLL_INTERVAL_SECONDS', 30_000),
        timeoutMs: seconds('NUGET_WAIT_TIMEOUT_SECONDS', 20 * minute)
    });
}
