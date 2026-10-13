// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useEffect, useState } from 'react';
import type { StringsDictionary } from '@cratis/scene.engine';
import { useStageSource } from './stageSource';

export interface Strings {
    /** Every locale at least one `.strings` file next to the model declares. */
    locales: string[];

    /** The active locale, or an empty string before the first `/stage/locales` fetch resolves. */
    locale: string;

    /** Switches the active locale, triggering a fetch of its dictionary. */
    setLocale: (locale: string) => void;

    /** The active locale's merged dictionary - empty until its fetch resolves. */
    dictionary: StringsDictionary;
}

/**
 * Fetches the locales a running Stage's `.strings` files declare, then the active one's dictionary -
 * resolution itself (turning a dictionary and a tree into a resolved one) is `resolveStringsInElement`
 * from `@cratis/scene.engine`; this only owns getting a dictionary from the network in the first place.
 *
 * The first locale `/stage/locales` reports becomes the default - there is no configured "default locale"
 * concept yet, so the model deciding which `.strings` file exists first (alphabetically, since
 * `IStringsFiles.FindIn` orders by relative path) is what a running Stage actually offers today.
 */
export function useStrings(): Strings {
    const source = useStageSource();
    const [locales, setLocales] = useState<string[]>([]);
    const [locale, setLocale] = useState('');
    const [dictionary, setDictionary] = useState<StringsDictionary>({});

    useEffect(() => {
        const abort = new AbortController();
        source.locales(abort.signal)
            .then(available => {
                setLocales(available);
                setLocale(current => current || available[0] || '');
            })
            .catch(() => { /* No .strings files, or the model has none yet - $strings. references stay literal. */ });
        return () => abort.abort();
    }, [source]);

    useEffect(() => {
        if (!locale) return;
        const abort = new AbortController();
        source.strings(locale, abort.signal)
            .then(setDictionary)
            .catch(() => setDictionary({}));
        return () => abort.abort();
    }, [locale, source]);

    return { locales, locale, setLocale, dictionary };
}
