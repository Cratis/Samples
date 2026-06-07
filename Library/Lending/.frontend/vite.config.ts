// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/// <reference types="vitest/config" />

import { defineConfig } from 'vitest/config';
import react from "@vitejs/plugin-react";
import { fileURLToPath } from 'node:url';
import { EmitMetadataPlugin } from '@cratis/arc.vite';
import tailwindcss from "@tailwindcss/vite";

export default defineConfig({
    root: fileURLToPath(new URL('./', import.meta.url)),
    optimizeDeps: {
        exclude: ['tslib']
    },
    esbuild: {
        supported: {
            'top-level-await': true,
        },
    },
    build: {
        outDir: '../wwwroot',
        assetsDir: '',
        modulePreload: false,
        target: 'esnext',
        minify: false,
        cssCodeSplit: false,
        rollupOptions: {
            external: [
            ],
        },
    },
    test: {
        globals: true,
        environment: 'node',
        isolate: false,
        fileParallelism: false,
        pool: 'threads',
        coverage: {
            provider: 'v8',
            exclude: [
                '**/for_*/**',
                '**/wwwroot/**',
                '**/dist/**',
                '**/*.test.tsx',
                '**/*.d.ts',
                '**/declarations.ts',
            ],
        },
        exclude: ['../dist/**', '../node_modules/**', 'node_modules/**', '../wwwroot/**', 'wwwroot/**', '../**/given/**'],
        include: ['../**/for_*/when_*/**/*.ts', '../**/for_*/**/when_*.ts'],
        setupFiles: fileURLToPath(new URL('../../../.frontend/vitest.setup.ts', import.meta.url))
    },
    plugins: [
        react(),
        tailwindcss(),
        EmitMetadataPlugin({ tsconfigPath: fileURLToPath(new URL('./tsconfig.json', import.meta.url)) }) as any
    ],
    server: {
        port: 9000,
        host: true,
        open: false,
        proxy: {
            '/api': {
                target: 'http://localhost:5000',
                ws: true
            },
            '/swagger': {
                target: 'http://localhost:5000'
            }
        }
    },
    resolve: {
        alias: {
            'Components': fileURLToPath(new URL('../Components', import.meta.url)),
            'Layout':     fileURLToPath(new URL('../Layout', import.meta.url)),
            'Authors':    fileURLToPath(new URL('../Authors', import.meta.url)),
            'Inventory':  fileURLToPath(new URL('../Inventory', import.meta.url)),
            'Lending':    fileURLToPath(new URL('../Lending', import.meta.url)),
        }
    }
});
