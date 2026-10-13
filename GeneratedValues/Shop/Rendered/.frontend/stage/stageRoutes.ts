// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useEffect, useState } from 'react';
import { stageEndpoints, useStageSource } from './stageSource';

/**
 * Where the running application registered its modeled commands and queries.
 */
export interface StageRoutes {
    commands: Record<string, string>;
    queries: Record<string, string>;
}

export interface StageRouteState {
    routes: StageRoutes | undefined;
    ready: boolean;
}

export const stageRoutes = stageEndpoints.routes;

/**
 * Reads the routes the Stage registered and says when that lookup has finished.
 *
 * An element carries the route it is backed by, but an interaction only names a command - so running one needs
 * this lookup. Readiness is separate from the routes object because an empty or failed route lookup is a real
 * loaded state; guessing a URL while it is still pending would turn a valid command into a transient 404.
 */
export function useStageRouteState(): StageRouteState {
    const source = useStageSource();
    const [state, setState] = useState<StageRouteState>({ routes: undefined, ready: false });

    useEffect(() => {
        const abort = new AbortController();
        source.routes(abort.signal)
            .then(routes => {
                if (!abort.signal.aborted) setState({ routes, ready: true });
            })
            .catch(() => {
                if (!abort.signal.aborted) setState({ routes: undefined, ready: true });
            });
        return () => abort.abort();
    }, [source]);

    return state;
}

/** Reads the routes the Stage registered. */
export function useStageRoutes(): StageRoutes | undefined {
    return useStageRouteState().routes;
}
