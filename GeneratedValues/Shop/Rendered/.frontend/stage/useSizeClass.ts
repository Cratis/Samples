// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useEffect, useState } from 'react';
import type { SizeClass } from '@cratis/scene.model';
import { computeSizeClass } from '@cratis/scene.engine';

/**
 * Tracks the browser window's {@link SizeClass}, recomputing whenever it resizes across a boundary the
 * shared engine cares about. This is what lets a modeled `when width compact` override actually take
 * effect on a narrow window instead of the arrangement engine's size-aware logic never being asked.
 */
export function useSizeClass(): SizeClass {
    const [sizeClass, setSizeClass] = useState<SizeClass>(() => computeSizeClass(globalThis.innerWidth, globalThis.innerHeight));

    useEffect(() => {
        const onResize = () => setSizeClass(computeSizeClass(globalThis.innerWidth, globalThis.innerHeight));
        globalThis.addEventListener('resize', onResize);
        return () => globalThis.removeEventListener('resize', onResize);
    }, []);

    return sizeClass;
}
