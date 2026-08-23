// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { access, readFile, stat } from 'node:fs/promises';
import path from 'node:path';
import { fileURLToPath } from 'node:url';

const scriptDirectory = path.dirname(fileURLToPath(import.meta.url));
const repositoryRoot = path.resolve(scriptDirectory, '..');
const catalogPath = path.join(repositoryRoot, 'samples.json');
let catalog;
try {
    catalog = JSON.parse(await readFile(catalogPath, 'utf8'));
} catch (error) {
    console.error(`Could not read ${catalogPath}:`, error.message);
    process.exit(1);
}
const errors = [];

const requireText = (value, location) => {
    if (typeof value !== 'string' || value.trim().length === 0) {
        errors.push(`${location} must be a non-empty string`);
    }
};

const requireStringList = (value, location) => {
    if (!Array.isArray(value) || value.length === 0) {
        errors.push(`${location} must contain at least one entry`);
        return;
    }

    value.forEach((entry, index) => requireText(entry, `${location}[${index}]`));
};

if (catalog.schemaVersion !== 1) {
    errors.push('schemaVersion must be 1');
}

if (!Array.isArray(catalog.tracks) || catalog.tracks.length === 0) {
    errors.push('tracks must contain at least one track');
}

if (!Array.isArray(catalog.samples) || catalog.samples.length === 0) {
    errors.push('samples must contain at least one sample');
}

const trackIds = new Set();
for (const [index, track] of (catalog.tracks ?? []).entries()) {
    const location = `tracks[${index}]`;
    requireText(track.id, `${location}.id`);
    requireText(track.title, `${location}.title`);
    requireText(track.description, `${location}.description`);

    if (trackIds.has(track.id)) {
        errors.push(`${location}.id duplicates '${track.id}'`);
    }
    trackIds.add(track.id);
}

const sampleIds = new Set();
for (const [index, sample] of (catalog.samples ?? []).entries()) {
    const location = `samples[${index}]`;
    for (const field of ['id', 'title', 'tagline', 'track', 'path', 'sourceUrl', 'ui', 'level']) {
        requireText(sample[field], `${location}.${field}`);
    }

    for (const field of ['products', 'runtime', 'prerequisites', 'highlights', 'verification', 'limitations']) {
        requireStringList(sample[field], `${location}.${field}`);
    }

    if (sample.previewUrl || sample.previewLabel) {
        requireText(sample.previewUrl, `${location}.previewUrl`);
        requireText(sample.previewLabel, `${location}.previewLabel`);
    }

    if (sampleIds.has(sample.id)) {
        errors.push(`${location}.id duplicates '${sample.id}'`);
    }
    sampleIds.add(sample.id);

    if (!trackIds.has(sample.track)) {
        errors.push(`${location}.track references unknown track '${sample.track}'`);
    }

    const sampleDirectory = path.resolve(repositoryRoot, sample.path ?? '');
    const relativePath = path.relative(repositoryRoot, sampleDirectory);
    if (relativePath.startsWith('..') || path.isAbsolute(relativePath)) {
        errors.push(`${location}.path must stay inside the repository`);
        continue;
    }

    try {
        const sampleStats = await stat(sampleDirectory);
        if (!sampleStats.isDirectory()) {
            errors.push(`${location}.path is not a directory: ${sample.path}`);
        }
    } catch {
        errors.push(`${location}.path does not exist: ${sample.path}`);
        continue;
    }

    try {
        await access(path.join(sampleDirectory, 'README.md'));
    } catch {
        errors.push(`${location}.path must contain README.md: ${sample.path}`);
    }
}

if (errors.length > 0) {
    console.error('Sample catalog validation failed:\n');
    errors.forEach(error => console.error(`  - ${error}`));
    process.exit(1);
}

console.log(`Validated ${catalog.samples.length} samples across ${catalog.tracks.length} tracks.`);
