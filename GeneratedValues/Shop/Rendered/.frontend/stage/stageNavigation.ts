// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/**
 * The parameters a screen was navigated to with, read from the hash (`#/WorkItemDetails?workItemId=...`).
 *
 * A modeled table that navigates on a row click names the parameter it navigates by. Keeping that parameter in
 * the address is what makes the selection a deep link: reloading, sharing or reopening the address selects the
 * same row and re-runs the queries that depend on it.
 *
 * @param hash The location hash.
 * @returns The parameters, by name.
 */
export function screenParameters(hash: string): Record<string, string> {
    const query = hash.indexOf('?');
    if (query < 0) return {};
    return Object.fromEntries(new URLSearchParams(hash.slice(query + 1)).entries());
}

/**
 * The hash addressing a screen with the given parameters.
 *
 * @param screen The screen name.
 * @param parameters The parameters to carry; empty values are left out.
 * @returns The hash.
 */
export function screenHashWith(screen: string, parameters: Record<string, string> = {}): string {
    const query = new URLSearchParams(Object.entries(parameters).filter(([, value]) => value !== ''));
    const serialized = query.toString();
    return serialized ? `#/${encodeURIComponent(screen)}?${serialized}` : `#/${encodeURIComponent(screen)}`;
}

/**
 * Navigates to a screen with parameters through the address, so the application, history and a reload agree.
 *
 * @param screen The screen name.
 * @param parameters The parameters to carry.
 */
export function navigateToScreen(screen: string, parameters: Record<string, string> = {}) {
    if (!globalThis.location) return;
    const hash = screenHashWith(screen, parameters);
    if (globalThis.location.hash === hash) return;
    globalThis.location.hash = hash;
}

/**
 * Turns an interaction's navigation arguments into screen parameters: every argument with a value, as text, the way
 * it appears in the address. An argument without a value is left out rather than written as `undefined`.
 * @param args The arguments the navigation was given.
 * @returns The screen parameters.
 */
export function screenArguments(args: Record<string, unknown> | undefined): Record<string, string> {
    return Object.fromEntries(Object.entries(args ?? {})
        .filter(([, value]) => value !== undefined && value !== null && value !== '' && typeof value !== 'object')
        .map(([name, value]) => [name, String(value)]));
}
