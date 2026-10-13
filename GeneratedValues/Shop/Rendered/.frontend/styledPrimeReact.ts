// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { readFile } from 'node:fs/promises';
import type { Plugin } from 'vite';

const primitive = /^primereact\/([a-z]+)$/;
const styledPrefix = '\0stage-styled-primereact:';

/**
 * Serves PrimeReact's styled components wherever its unstyled primitives are imported.
 *
 * `primereact/<component>` is PrimeReact 11's headless layer: it renders the markup but carries no styles, so
 * nothing positions or paints it. The styled components - the same API with their style modules applied - live
 * in `@primereact/ui/<component>`. The Scene components Stage renders with (`@cratis/scene.primereact`) import
 * the primitives, which left a Stage application's buttons as bare text and a command's dialog rendered as plain
 * content below the screen, where the user never saw it open.
 *
 * An import of a primitive from outside PrimeReact therefore gets a module that re-exports everything the
 * primitive does, with every name `@primereact/ui` also exports taken from there instead. The styled package
 * does not cover every helper a primitive exports, which is why it cannot simply replace the primitive.
 * PrimeReact's own packages are built on the primitives - `@primereact/ui` wraps them and the primitives import
 * one another - so imports from inside PrimeReact are left alone.
 */
export function styledPrimeReact(): Plugin {
    return {
        name: 'stage-styled-primereact',
        enforce: 'pre',
        resolveId(source, importer) {
            const match = primitive.exec(source);
            if (!match || !importer || importer.startsWith(styledPrefix) || /\/node_modules\/(primereact|@primereact)\//.test(importer)) {
                return null;
            }

            return `${styledPrefix}${match[1]}`;
        },
        async load(id) {
            if (!id.startsWith(styledPrefix)) {
                return null;
            }

            const component = id.slice(styledPrefix.length);
            const styled = await this.resolve(`@primereact/ui/${component}`, undefined, { skipSelf: true });
            if (!styled) {
                return `export * from 'primereact/${component}';`;
            }

            const names = exportedNames(await readFile(styled.id, 'utf8'));
            return [
                `export * from 'primereact/${component}';`,
                names.length > 0 ? `export { ${names.join(', ')} } from '@primereact/ui/${component}';` : '',
            ].join('\n');
        },
    };
}

/**
 * Reads the names a bundled ES module exports from its `export { ... }` statements.
 * @param source The module source.
 * @returns The exported names.
 */
export function exportedNames(source: string): string[] {
    const names = new Set<string>();
    for (const statement of source.matchAll(/export\s*\{([^}]*)\}/g)) {
        for (const specifier of statement[1].split(',')) {
            const name = specifier.trim().split(/\s+as\s+/).pop()?.trim();
            if (name && name !== 'default') {
                names.add(name);
            }
        }
    }

    return [...names];
}
