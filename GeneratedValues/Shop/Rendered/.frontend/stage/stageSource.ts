// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { createContext, createElement, useContext, type ReactNode } from 'react';
import type { StringsDictionary } from '@cratis/scene.engine';
import type { StageRoutes } from './stageRoutes';
import type { StageSceneApplication } from './App';

/**
 * Where the runtime reads the application it renders: the Scene document, the routes the modeled commands and
 * queries are registered under, and the localized strings.
 *
 * A live Stage serves these over HTTP. A generated application already holds them when it is built - the
 * renderer planned the document and the strings, and the generated Arc proxies carry the routes - so it hands
 * them over directly. Both run the same runtime; only where the four answers come from differs.
 */
export interface StageSource {
    /** Reads the Scene document to render. Rejects when it cannot be read. */
    scene(signal: AbortSignal): Promise<StageSceneApplication>;

    /** Reads the registered routes, or `undefined` when there are none to read. */
    routes(signal: AbortSignal): Promise<StageRoutes | undefined>;

    /** Reads the locales the application's strings declare. */
    locales(signal: AbortSignal): Promise<string[]>;

    /** Reads one locale's dictionary. */
    strings(locale: string, signal: AbortSignal): Promise<StringsDictionary>;
}

/** The Stage endpoints, relative to the page so a Stage served under a base path still finds them. */
export const stageEndpoints = {
    scene: 'stage/scene',
    routes: 'stage/routes',
    locales: 'stage/locales',
    strings: (locale: string) => `stage/strings/${encodeURIComponent(locale)}`,
};

/** Reads everything from the endpoints a running Stage host serves. */
export const httpStageSource: StageSource = {
    scene: async signal => {
        const response = await fetch(stageEndpoints.scene, { signal });
        if (!response.ok) throw new Error(`The Stage returned ${response.status}.`);
        return response.json() as Promise<StageSceneApplication>;
    },
    routes: async signal => {
        const response = await fetch(stageEndpoints.routes, { signal });
        return response.ok ? response.json() as Promise<StageRoutes> : undefined;
    },
    locales: async signal => {
        const response = await fetch(stageEndpoints.locales, { signal });
        return response.ok ? response.json() as Promise<string[]> : [];
    },
    strings: async (locale, signal) => {
        const response = await fetch(stageEndpoints.strings(locale), { signal });
        return response.ok ? response.json() as Promise<StringsDictionary> : {};
    },
};

/** What a generated application already holds when it is built. */
export interface StageStaticContent {
    scene: StageSceneApplication;
    routes: StageRoutes;
    strings: Record<string, StringsDictionary>;
}

/**
 * Serves content that was planned and built into the application. Locales are the keys of `strings`, in the
 * order the renderer wrote them; an unknown locale has an empty dictionary, as an unknown one does over HTTP.
 * @param content The planned document, routes and strings.
 * @returns A source answering from that content without any request.
 */
export function staticStageSource(content: StageStaticContent): StageSource {
    return {
        scene: async () => content.scene,
        routes: async () => content.routes,
        locales: async () => Object.keys(content.strings),
        strings: async locale => content.strings[locale] ?? {},
    };
}

const StageSourceContext = createContext<StageSource>(httpStageSource);

/** Supplies the source the runtime below reads from. Without one, the runtime reads the Stage endpoints. */
export function StageSourceProvider({ source, children }: { source: StageSource; children?: ReactNode }) {
    return createElement(StageSourceContext.Provider, { value: source }, children);
}

/** The source the runtime reads from. */
export function useStageSource(): StageSource {
    return useContext(StageSourceContext);
}
