// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { createContext, useContext, type ReactNode } from 'react';
import type { FlowArrangement } from '@cratis/scene.model';
import { evaluateFlowArrangement } from '@cratis/scene.engine';
import type { ComponentRegistry, RegisteredComponentProps } from '@cratis/scene.react';
import { SceneElementView, SceneNavigationContextInternal, useSceneOutlets, useSceneTheme } from '@cratis/scene.react';
import { useEffect } from 'react';
import { FlowArrangementView } from './FlowArrangementView';
import { useSizeClass } from './useSizeClass';
import { screenHash, stageLocaleComponent, stageNavigationTreeComponent } from './blueprint';
import { opensElsewhere, type NavigationNode } from './navigationTree';
import { useStageData } from './stageData';

/** What the Stage's own components need that the document itself does not carry. */
export interface StageChromeState {
    locales: string[];
    locale: string;
    setLocale: (locale: string) => void;
    activity: string;
}

const emptyState: StageChromeState = { locales: [], locale: '', setLocale: () => undefined, activity: '' };

const StageChromeContext = createContext<StageChromeState>(emptyState);

/** Provides {@link StageChromeState} to the Stage's own components. */
export function StageChromeProvider({ state, children }: { state: StageChromeState; children: ReactNode }) {
    return <StageChromeContext.Provider value={state}>{children}</StageChromeContext.Provider>;
}

/** The name the template wrapper is registered under. */
export const stageTemplateComponent = 'Stage:template';

/** The name the status line is registered under. */
export const stageActivityComponent = 'Stage:activity';

function isFlowArrangement(arrangement: unknown): arrangement is FlowArrangement {
    return !!arrangement && typeof arrangement === 'object' && 'root' in arrangement;
}

/**
 * A screen's content inside the screen template it names.
 *
 * The template's own `arrangement flow` positions its slots - header, body, aside - exactly as the document
 * wrote it, evaluated for the window's current size class. A template with no flow arrangement stacks them.
 * This sits inside the blueprint shell's content slot, so the shell, not this, owns the chrome around it.
 */
export function StageTemplate({ element, slots }: RegisteredComponentProps) {
    const sizeClass = useSizeClass();
    const arrangement = (element.properties as Record<string, unknown> | undefined)?.arrangement;
    const nodes: Record<string, ReactNode> = {};
    for (const [name, content] of Object.entries(slots)) {
        nodes[name] = <section className='stage-slot' data-slot={name}>{content}</section>;
    }

    return (
        <div className='stage-template' data-scene-id={element.id}>
            {isFlowArrangement(arrangement)
                ? <FlowArrangementView node={evaluateFlowArrangement(arrangement, sizeClass)} slots={nodes} />
                : Object.entries(nodes).map(([name, node]) => <div key={name}>{node}</div>)}
        </div>
    );
}

/** The language picker in the topbar, shown only when the model declares more than one. */
export function StageLocale() {
    const { locales, locale, setLocale } = useContext(StageChromeContext);
    if (locales.length < 2) return null;

    return (
        <select className='stage-locale' aria-label='Locale' value={locale} onChange={event => setLocale(event.target.value)}>
            {locales.map(candidate => <option key={candidate} value={candidate}>{candidate}</option>)}
        </select>
    );
}

/**
 * The application's navigation as the hierarchy the model declared: a group per module and feature, nested as deep
 * as the model nests them, each entry linking to the screen it opens.
 */
export function StageNavigationTree({ element }: RegisteredComponentProps) {
    const properties = (element.properties ?? {}) as { nodes?: NavigationNode[]; activeScreen?: string };
    return (
        <nav className='stage-navigation-tree' aria-label='Navigation' data-scene-id={element.id}>
            <NavigationLevel nodes={properties.nodes ?? []} activeScreen={properties.activeScreen} />
        </nav>
    );
}

function NavigationLevel({ nodes, activeScreen }: { nodes: NavigationNode[]; activeScreen?: string }) {
    return (
        <ul role='tree'>
            {nodes.map(node => node.screen !== undefined
                ? (
                    <li key={`${node.screen}:${node.label}`} role='treeitem'>
                        <NavigationEntryLink node={node} screen={node.screen} activeScreen={activeScreen} />
                    </li>
                )
                : (
                    <li key={node.label} role='treeitem' aria-expanded='true'>
                        <span className='stage-navigation-group'>{node.label}</span>
                        <NavigationLevel nodes={node.children} activeScreen={activeScreen} />
                    </li>
                ))}
        </ul>
    );
}

/**
 * One navigation entry as the model authored it.
 *
 * The presentation hint decides what is shown - `icon`, `text` or `both` (the default); an icon-only entry keeps its
 * label as its accessible name. An entry that opens somewhere a plain link does not reach - an outlet, a dialog - is
 * executed through the Scene navigation host, the same as a toolbar item; an external route is an ordinary link.
 */
function NavigationEntryLink({ node, screen, activeScreen }: { node: NavigationNode; screen: string; activeScreen?: string }) {
    const navigation = useContext(SceneNavigationContextInternal);
    const mode = node.presentation === 'icon' || node.presentation === 'text' ? node.presentation : 'both';
    const destination = node.destination;
    const external = destination?.kind === 'external' && destination.route !== undefined;
    const execute = destination && navigation && !external && opensElsewhere(destination)
        ? (event: { preventDefault: () => void }) => {
            event.preventDefault();
            navigation.navigate(destination);
        }
        : undefined;

    return (
        <a
            href={external ? destination.route : screenHash(screen)}
            rel={external ? 'noreferrer' : undefined}
            aria-current={screen === activeScreen ? 'page' : undefined}
            aria-label={mode === 'icon' ? node.label : undefined}
            data-presentation={node.presentation}
            data-icon={node.icon}
            onClick={execute}>
            {mode !== 'text' && node.icon && <span className='stage-navigation-icon' aria-hidden='true' data-icon-name={node.icon} />}
            {mode !== 'icon' && node.label}
        </a>
    );
}

/**
 * The `core:outlet` the Stage renders: the screen the Scene navigation host placed in the named outlet, drawn
 * inside the Stage's own data scope so its queries, selections and commands work as they do on a primary screen.
 *
 * Scene's own outlet renders the placed screen in a scope of its own, which cannot see what the Stage resolves
 * queries and selections with, so the Stage draws the same region itself.
 */
export function StageOutlet({ element }: RegisteredComponentProps) {
    const name = typeof element.properties.name === 'string' ? element.properties.name : '';
    const navigation = useContext(SceneNavigationContextInternal);
    const outlets = useSceneOutlets();
    const data = useStageData();
    const placed = navigation?.outlets[name];
    const screen = placed && outlets ? outlets.screens[placed] : undefined;

    return (
        <div data-scene-outlet={name} data-scene-outlet-screen={placed}>
            {screen && outlets && <SceneElementView element={screen} registry={outlets.registry} resolveBinding={data.resolveBinding} />}
        </div>
    );
}

/** What the last interaction did, or could not do. */
export function StageActivity() {
    const { activity } = useContext(StageChromeContext);
    return activity ? <p className='stage-activity' role='status'>{activity}</p> : null;
}

/**
 * Mirrors the active theme's colour scheme and page colors onto the document.
 *
 * The theme provider scopes its tokens to its own element, but PrimeReact portals a dialog or an overlay to
 * `body`, outside that element, and the page behind the shell is `body` too. Without this, a dark theme would
 * paint the shell dark, leave the page around it light and open every dialog light.
 */
export function ColorSchemeMirror() {
    const theme = useSceneTheme();
    const background = theme?.tokens?.['surface.background'];
    const color = theme?.tokens?.['text.color'];
    useEffect(() => {
        const scheme = theme?.isDark ? 'dark' : 'light';
        document.documentElement.setAttribute('data-scene-color-scheme', scheme);
        document.body.setAttribute('data-scene-color-scheme', scheme);
        document.body.style.backgroundColor = background ?? '';
        document.body.style.color = color ?? '';
    }, [theme?.isDark, background, color]);

    return null;
}

/** The Stage's own components, to merge over the blueprint's. */
export const stageChromeComponents: ComponentRegistry = {
    [stageTemplateComponent]: StageTemplate,
    [stageLocaleComponent]: StageLocale,
    [stageNavigationTreeComponent]: StageNavigationTree,
    [stageActivityComponent]: StageActivity,
    'core:outlet': StageOutlet,
};
