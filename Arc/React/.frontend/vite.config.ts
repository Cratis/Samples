// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/// <reference types="vitest/config" />

import { EmitMetadataPlugin } from '@cratis/arc.vite';
import react from '@vitejs/plugin-react';
import { fileURLToPath } from 'node:url';
import { defineConfig, type PluginOption } from 'vite';

const backend = 'http://localhost:5064';

export default defineConfig({
    root: fileURLToPath(new URL('./', import.meta.url)),
    build: {
        outDir: '../wwwroot',
        emptyOutDir: true,
        assetsDir: 'assets',
        target: 'esnext',
        modulePreload: false,
        chunkSizeWarningLimit: 700,
    },
    plugins: [
        react(),
        // SAFETY: Arc and this app share Vite's plugin lifecycle; the cast only bridges their bundled Vite type identities.
        EmitMetadataPlugin({
            tsconfigPath: fileURLToPath(new URL('./tsconfig.json', import.meta.url)),
        }) as unknown as PluginOption,
    ],
    server: {
        host: true,
        port: 5173,
        open: false,
        proxy: {
            '/api': {
                target: backend,
                ws: true,
            },
            '/.cratis': {
                target: backend,
                ws: true,
            },
        },
    },
    test: {
        globals: true,
        environment: 'node',
        include: ['../**/for_*/when_*/**/*.ts', '../**/for_*/when_*.ts'],
        exclude: ['../wwwroot/**', '../bin/**', '../obj/**', '../node_modules/**'],
        setupFiles: fileURLToPath(new URL('../../../.frontend/vitest.setup.ts', import.meta.url)),
    },
});
