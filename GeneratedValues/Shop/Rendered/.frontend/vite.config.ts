import { defineConfig, type PluginOption } from 'vite';
import react from '@vitejs/plugin-react';
import { fileURLToPath } from 'node:url';
import { EmitMetadataPlugin } from '@cratis/arc.vite';
import { styledPrimeReact } from './styledPrimeReact';

export default defineConfig({
    root: fileURLToPath(new URL('./', import.meta.url)),
    envPrefix: 'STAGE_',
    define: {
        'import.meta.env.STAGE_PRIMEUI_LICENSE': JSON.stringify(process.env.STAGE_PRIMEUI_LICENSE || process.env.PRIMEUI_LICENSE || ''),
    },
    optimizeDeps: {
        exclude: ['tslib'],
    },
    build: {
        outDir: '../wwwroot',
        modulePreload: false,
        target: 'esnext',
        minify: false,
        cssCodeSplit: false,
    },
    plugins: [
        styledPrimeReact(),
        react(),
        EmitMetadataPlugin({ tsconfigPath: fileURLToPath(new URL('./tsconfig.json', import.meta.url)) }) as PluginOption,
    ],
    server: {
        port: 9000,
        open: true,
        proxy: {
            '/.cratis': {
                target: 'http://localhost:5000',
                ws: true
            },
            '/api': {
                target: 'http://localhost:5000',
                ws: true
            },
            '/swagger': {
                target: 'http://localhost:5000',
                ws: true
            }
        }
    }
});
