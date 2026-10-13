// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import type { CSSProperties, ReactNode } from 'react';
import { FlowContainerKind, type FlowNode } from '@cratis/scene.model';
import { isFlowContainer, isFlowGrid, isFlowSlotLeaf } from '@cratis/scene.engine';

/**
 * Renders a {@link FlowNode} tree - the same one `evaluateFlowArrangement` picks for the current size
 * class - by walking it into nested flex/grid containers and dropping each named slot's real content into
 * its {@link FlowSlotLeaf}. This is what turns a modeled `arrangement flow` (a layout's topbar/sidebar/
 * content/footer, or a screen template's header/body/aside) into something that actually looks like more
 * than one column, rather than every slot's content stacked flat regardless of what the document said.
 *
 * A fixed `width`/`height` on a slot is a known, documented gap in the arrangement it is fed (see
 * `ArrangementConverter` in Stage) - only `grow` and `span` survive conversion. A slot that does not grow
 * therefore sizes to its own content here, which is a reasonable rendering of "not growing" even though
 * the exact pixel the author wrote is not carried this far yet.
 */
export function FlowArrangementView({ node, slots }: { node: FlowNode; slots: Record<string, ReactNode> }) {
    if (isFlowSlotLeaf(node)) {
        return <>{slots[node.slotName] ?? null}</>;
    }

    if (isFlowGrid(node)) {
        const style: CSSProperties = {
            gap: `${node.gap}px`,
            gridTemplateColumns: node.columns ? `repeat(${node.columns}, 1fr)` : undefined,
            gridTemplateRows: node.rows ? `repeat(${node.rows}, 1fr)` : undefined,
        };
        return (
            <div className='flow-container flow-container--grid' style={style}>
                {node.children.map((child, index) => <FlowChild key={index} node={child} slots={slots} />)}
            </div>
        );
    }

    if (isFlowContainer(node)) {
        const direction = node.kind === FlowContainerKind.Row ? 'row' : 'column';
        return (
            <div className={`flow-container flow-container--${direction}`} style={{ gap: `${node.gap}px` }}>
                {node.children.map((child, index) => <FlowChild key={index} node={child} slots={slots} />)}
            </div>
        );
    }

    return null;
}

function FlowChild({ node, slots }: { node: FlowNode; slots: Record<string, ReactNode> }) {
    const style: CSSProperties = node.grow
        ? { flex: `${node.grow} 1 0%`, minWidth: 0, minHeight: 0 }
        : { flex: '0 0 auto' };
    return (
        <div className='flow-child' style={style}>
            <FlowArrangementView node={node} slots={slots} />
        </div>
    );
}
