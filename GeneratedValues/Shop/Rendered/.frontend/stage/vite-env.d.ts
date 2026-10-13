// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/// <reference types="vite/client" />

interface ImportMetaEnv {
    /**
     * The PrimeUI license key, remapped from the `PRIMEUI_LICENSE` GitHub secret by `vite.config.ts`'s
     * `envPrefix` - see `main.tsx` for where it is read.
     */
    readonly STAGE_PRIMEUI_LICENSE?: string;
}

interface ImportMeta {
    readonly env: ImportMetaEnv;
}
