// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import type { SceneElement, Screen, ScreenTemplate, UiProfile } from '@cratis/scene.model';
import { authoredNavigationEntries, navigationPathTo, navigationTree, unlistedNavigationEntries } from './navigationTree';
import type { PackageCatalog } from '@cratis/scene.engine';
import { coreComponents, type ComponentRegistry } from '@cratis/scene.react';
import { primeReactComponents, primeReactPackageManifest } from '@cratis/scene.primereact';
import {
    ComponentName,
    SlotName,
    defaultBlueprintComponents,
    defaultBlueprintName,
    defaultBlueprintThemes,
    externalComponent,
    galleryScreenTemplates,
    navigationElement,
    resolveElementComponentNames,
} from '@cratis/scene.blueprint.default';

/** The package every Stage application resolves bare component names against, in ascending priority. */
const stagePackages = ['core', 'PrimeReact', defaultBlueprintName];

/** The qualified name Stage registers its locale picker under. */
export const stageLocaleComponent = 'Stage:locale';

/** The qualified name Stage registers its navigation tree under. */
export const stageNavigationTreeComponent = 'Stage:navigationTree';

/**
 * The profile a scene renders against.
 *
 * The model's own `ui profile` decides which packages it draws from, but Stage always renders through the
 * default blueprint, so that package - and the PrimeReact widgets it is built from - are admitted even when a
 * model never named them. Without that, a template written against the blueprint's vocabulary would come out
 * as a column of dashed placeholders.
 *
 * @param profiles The profiles the scene declares.
 * @returns The profile to resolve component names with.
 */
export function stageProfile(profiles: UiProfile[] | undefined): UiProfile {
    const declared = profiles?.find(profile => profile.targetPlatform === 'web') ?? profiles?.[0];
    const packages = [...(declared?.packages ?? [])];
    for (const required of stagePackages) {
        if (!packages.includes(required)) packages.push(required);
    }

    return { name: declared?.name ?? 'Stage', targetPlatform: declared?.targetPlatform ?? 'web', packages, defaultSizeClass: declared?.defaultSizeClass, layout: declared?.layout, theme: declared?.theme };
}

/** What each package declares, so a bare name resolves to the package that provides it. */
export const stageCatalog: PackageCatalog = {
    core: Object.keys(coreComponents).map(key => key.slice(key.indexOf(':') + 1)),
    PrimeReact: primeReactPackageManifest.components,
    [defaultBlueprintName]: Object.values(ComponentName),
};

/** Every component the Stage can render: Scene's core, PrimeReact, and the default blueprint's shell. */
export function stageRegistry(overrides: ComponentRegistry): ComponentRegistry {
    return { ...coreComponents, ...primeReactComponents, ...defaultBlueprintComponents, ...overrides };
}

/**
 * The template a screen fills: the model's own, or one of the blueprint's.
 *
 * A model that writes `template CrudList` without declaring it is naming one of the blueprint's templates, so
 * its arrangement and slots come from there. A model's own declaration always wins the name.
 */
export function templateFor(templates: ScreenTemplate[], screen: Screen): ScreenTemplate | undefined {
    if (!screen.screenTemplate) return undefined;
    return templates.find(candidate => candidate.name === screen.screenTemplate)
        ?? galleryScreenTemplates.find(candidate => candidate.name === screen.screenTemplate);
}

/**
 * What every package the application renders with declares: the Stage's own packages, and the components of the
 * custom packages the application approved.
 *
 * A custom package never replaces what a Stage package declares; it only adds its own entry.
 *
 * @param approved What each approved custom package declares, by package name.
 * @returns The catalog component names are resolved against.
 */
export function stageCatalogWith(approved: PackageCatalog | undefined): PackageCatalog {
    return approved ? { ...approved, ...stageCatalog } : stageCatalog;
}

/**
 * Rewrites an element tree's bare component names into the keys the registry is keyed by.
 *
 * A name that is already qualified, or that no package declares, is left exactly as written; the renderer then
 * shows a placeholder naming what is missing rather than failing.
 *
 * @param element The tree to rewrite.
 * @param profile The profile whose package priority decides which package wins a bare name.
 * @param catalog What each package declares; defaults to the Stage's own packages.
 * @returns The rewritten tree.
 */
export function resolveNames(element: SceneElement, profile: UiProfile, catalog: PackageCatalog = stageCatalog): SceneElement {
    return resolveElementComponentNames(element, profile, catalog);
}

/** The name the application shows in its topbar and sidebar. */
export const stageApplicationName = 'Cratis Stage';

function logo(id: string): SceneElement {
    return externalComponent(id, ComponentName.Logo, { label: stageApplicationName, initials: 'S', targetScreen: '' });
}

/**
 * The chrome the blueprint's application shell wraps every screen in.
 *
 * Built from the screens the scene declares, because that is what an application's navigation is: one entry
 * per screen, the current one marked, and a breadcrumb back to the first.
 */
export function applicationChrome(screens: Screen[], activeScreen: string, locales: string[], locale: string): Record<string, SceneElement[]> {
    const first = screens[0]?.name ?? '';
    const authored = authoredNavigationEntries(screens);
    // A screen the model gave no navigation entry stays reachable - under its own name, in the module and feature it was
    // declared in, after the authored entries - so adding one entry never hides the rest of the application.
    const listed = new Set(authored.map(entry => entry.targetScreen));
    const unlisted = unlistedNavigationEntries(screens, listed, 'Other screens');
    const tree = navigationTree([...authored, ...(authored.length > 0 ? unlisted : [])]);
    const entries = screens.map((screen, index) => navigationElement({ label: screen.name, targetScreen: screen.name, icon: 'pi pi-table', group: 'Screens', order: index }, activeScreen));
    const authoredTrail = navigationPathTo(navigationTree(authored), activeScreen);
    // A screen no entry lists is still somewhere in the hierarchy when the model recorded where it was declared.
    const placed = screens.find(screen => screen.name === activeScreen)?.path?.length ? navigationPathTo(tree, activeScreen) : [];
    const trail = authoredTrail.length > 0 ? authoredTrail : placed;
    const menu = authored.length > 0
        ? externalComponent('menu-hierarchy', stageNavigationTreeComponent, { nodes: tree, activeScreen })
        : externalComponent('menu-screens', ComponentName.Menu, { title: 'Screens', label: 'Screens' }, { items: entries });
    const end: SceneElement[] = [];
    if (locales.length > 1) end.push(externalComponent('topbar-locale', stageLocaleComponent, { locales, locale }));
    end.push(externalComponent('topbar-theme', ComponentName.ThemeSwitcher, { label: 'Theme' }));

    return {
        [SlotName.Topbar]: [externalComponent('topbar', ComponentName.Topbar, {}, { logo: [logo('topbar-logo')], end })],
        [SlotName.Sidebar]: [externalComponent('sidebar', ComponentName.Sidebar, {}, { logo: [logo('sidebar-logo')] })],
        [SlotName.Menu]: [menu],
        [SlotName.Breadcrumb]: [externalComponent('breadcrumb', ComponentName.Breadcrumb, {
            homeTargetScreen: first,
            items: (trail.length > 0 ? trail : [activeScreen]).map(label => ({ label })),
        })],
        [SlotName.Footer]: [externalComponent('footer', ComponentName.Footer, { text: `${stageApplicationName} · rendered from your model` })],
        [SlotName.ConfigPanel]: [externalComponent(ComponentName.ConfigPanel, ComponentName.ConfigPanel, {
            title: 'Settings',
            themes: defaultBlueprintThemes.map(theme => ({ name: theme.name, label: theme.name, isDark: theme.isDark ?? false })),
        })],
    };
}

/** The hash a screen is addressed by - what the blueprint's menu and breadcrumb links point at. */
export function screenHash(name: string): string {
    return `#/${encodeURIComponent(name)}`;
}

/** The screen name a hash addresses, or undefined when it addresses none. Parameters after `?` are not part of it. */
export function screenFromHash(hash: string): string | undefined {
    const match = /^#\/([^?]+)/.exec(hash);
    if (!match) return undefined;
    try {
        return decodeURIComponent(match[1]);
    } catch {
        return undefined;
    }
}
