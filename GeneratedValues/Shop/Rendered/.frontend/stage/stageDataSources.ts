// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import type { ExternalComponent, SceneElement } from '@cratis/scene.model';

/** The component a modeled `data` declaration is rendered as. */
export const dataComponent = 'core:data';

const boundComponents = new Set(['core:table']);

/**
 * Connects every table to the modeled `data` declaration that feeds it.
 *
 * A screen declares its data once - `data CommentsForWorkItem by workItemId` - and the table that follows it in
 * the same section, or any section nested after it, shows that read model. The table itself only names the read
 * model type, so without this it would read the unfiltered collection: every comment of every work item. This
 * carries the declaration's query and its `by` parameter onto the table, bound to the screen parameter of the
 * same name, so the table reads only what the declaration asked for and reads nothing while that parameter has
 * no value.
 *
 * @param element The composed screen.
 * @returns The screen with every fed table bound to its declaration.
 */
export function bindDataSources(element: SceneElement): SceneElement {
    return bind(element, new Map(), undefined);
}

function bind(element: SceneElement, sources: Map<string, ExternalComponent>, nearest: ExternalComponent | undefined): SceneElement {
    const component = element as ExternalComponent;
    if (!component.slots) return element;

    let changed = false;
    const slots: Record<string, SceneElement[]> = {};
    for (const [name, children] of Object.entries(component.slots)) {
        const scope = new Map(sources);
        let subject = nearest;
        slots[name] = children.map(child => {
            const childComponent = child as ExternalComponent;
            const typeName = stringProperty(childComponent, 'typeName');
            if (childComponent.componentName === dataComponent && typeName) {
                scope.set(typeName, childComponent);
                subject = childComponent;
            }

            const fed = feedSubject(feed(childComponent, scope), subject);
            const bound = bind(fed, scope, subject);
            if (bound !== child) changed = true;
            return bound;
        });
    }

    return changed ? { ...component, slots } as SceneElement : element;
}

function feed(component: ExternalComponent, sources: Map<string, ExternalComponent>): ExternalComponent {
    if (!boundComponents.has(component.componentName)) return component;
    const typeName = stringProperty(component, 'typeName');
    const source = typeName ? sources.get(typeName) : undefined;
    if (!source || component.properties.queryArguments !== undefined) return component;

    const by = stringProperty(source, 'by');
    const properties: Record<string, unknown> = { ...component.properties };
    const query = stringProperty(source, 'query');
    if (query) properties.query = query;
    if (!stringProperty(component, 'route') && stringProperty(source, 'route')) properties.route = source.properties.route;
    if (by) {
        properties.by = by;
        properties.queryArguments = { [by]: { path: `parameters.${by}` } };
    }

    return { ...component, properties };
}

/**
 * Carries the nearest `data` declaration onto a guarded action as the item its guard is evaluated against: `item` in
 * `when item.status == "open"` is the single record that declaration reads, bound to the screen parameter it is read
 * by - the same declaration Screenplay requires a guarded action to have exactly one of.
 */
function feedSubject(component: ExternalComponent, subject: ExternalComponent | undefined): ExternalComponent {
    if (component.componentName !== 'core:action' || !Array.isArray(component.properties?.alternatives)) return component;
    if (!subject || subject.properties?.isCollection === true || component.properties.itemQuery !== undefined) return component;

    const properties: Record<string, unknown> = { ...component.properties };
    const query = stringProperty(subject, 'query');
    const by = stringProperty(subject, 'by');
    if (query) properties.itemQuery = query;
    if (stringProperty(subject, 'route')) properties.itemRoute = subject.properties.route;
    properties.itemArguments = by ? { [by]: { path: `parameters.${by}` } } : {};
    return { ...component, properties };
}

function stringProperty(component: ExternalComponent, name: string): string | undefined {
    const value = component.properties?.[name];
    return typeof value === 'string' && value.length > 0 ? value : undefined;
}
