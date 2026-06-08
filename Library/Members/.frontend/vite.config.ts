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
        port: 9001,
        host: true,
        open: false,
        allowedHosts: true,
        proxy: {
            '/api': {
                target: 'http://localhost:5001',
                ws: true
            },
            '/swagger': {
                target: 'http://localhost:5001'
            }
        }
    },
    resolve: {
        alias: {
            'Profiles': fileURLToPath(new URL('../Profiles', import.meta.url)),
        }
    }
});
