// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useEffect, useMemo, useState } from 'react';
import { DestinationKind, type DestinationReference, type DialogTemplate, type Form, type Layout, type SceneElement, type Screen, type ScreenTemplate, type Theme, type UiProfile } from '@cratis/scene.model';
import type { CommandOutcome, InteractionFinding, PackageCatalog } from '@cratis/scene.engine';
import { resolveStringsInElement } from '@cratis/scene.engine';
import {
    InteractionScope,
    SceneElementView,
    SceneNavigationProvider,
    SceneOutletContext,
    collectOutletOwners,
    createBrowserDispatcher,
    createHashSceneHistory,
    sceneRoutesFrom,
    useSceneHistoryNavigation,
    useSceneNavigation,
    type ComponentRegistry,
} from '@cratis/scene.react';
import {
    ComponentName,
    LayoutConfigProvider,
    LayoutThemeProvider,
    SlotName,
    externalComponent,
    shellComponentForLayout,
} from '@cratis/scene.blueprint.default';
import '@cratis/scene.blueprint.default/styles.css';
import { useStageRouteState, type StageRoutes } from './stageRoutes';
import { StageDataProvider, useStageData } from './stageData';
import { stageCommandFormComponent, stageComponents } from './stageComponents';
import { useStrings } from './useStrings';
import { ColorSchemeMirror, StageChromeProvider, stageActivityComponent, stageChromeComponents, stageTemplateComponent } from './StageChrome';
import { applicationChrome, resolveNames, stageCatalogWith, stageProfile, stageRegistry, templateFor } from './blueprint';
import { bindDataSources } from './stageDataSources';
import { navigateToScreen, screenArguments } from './stageNavigation';
import { executeStageCommand } from './stageCommands';
import { useStageSource } from './stageSource';
import './app.css';

export interface StageSceneApplication {
    uiProfiles?: UiProfile[];
    themes?: Theme[];
    layouts: Layout[];
    screenTemplates: ScreenTemplate[];
    dialogTemplates?: DialogTemplate[];
    screens: Screen[];
}

interface SceneCommandDetail {
    command: string;
}

interface SceneNavigateDetail {
    targetScreen: string;
    dialog?: string;
    destination?: DestinationReference;
}

/** What the Scene navigation host resolves a destination's route parameters against; the Stage's items resolve their own. */
const hostBindingScope = {};

/**
 * The registry a Stage renders with: Scene's core vocabulary, PrimeReact and the default blueprint, then any
 * components the host adds, then the Stage's own table, action, form and chrome - which always win, because they
 * are what reach the running application.
 * @param components Components the host adds, such as a package an application composes with.
 * @returns The registry.
 */
export function stageApplicationRegistry(components: ComponentRegistry = {}): ComponentRegistry {
    return stageRegistry({ ...components, ...stageComponents, ...stageChromeComponents });
}

/**
 * Composes one screen the way the default blueprint composes its own: the application shell for the screen's
 * layout, the chrome every screen shares in the shell's slots, and the screen - inside the screen template it
 * names, when it names one - in the shell's content slot.
 *
 * A layout the selected package set does not provide remains unresolved in the rendered tree. Showing that
 * missing component is deliberate: silently substituting a default shell would hide a package/profile problem
 * and produce a screen the model did not ask for.
 */
export function composeStageScreen(scene: StageSceneApplication, screen: Screen, locales: string[], locale: string, packageCatalog?: PackageCatalog): SceneElement {
    const profile = stageProfile(scene.uiProfiles);
    const content = screenContent(scene, screen);

    const shell = shellComponentForLayout(screen.layout) ?? screen.layout;
    const chrome = shell === ComponentName.FullPageShell
        ? { [SlotName.ConfigPanel]: applicationChrome(scene.screens, screen.name, locales, locale)[SlotName.ConfigPanel] }
        : applicationChrome(scene.screens, screen.name, locales, locale);
    const composed = externalComponent(`screen-${screen.name}`, shell, { screenName: screen.name }, {
        ...chrome,
        [SlotName.Content]: [...content, externalComponent('activity', stageActivityComponent)],
    });

    return resolveNames(bindDataSources(composed), profile, stageCatalogWith(packageCatalog));
}

/**
 * What a screen puts in the shell's content slot: the screen - inside the screen template it names, when it names
 * one - and the forms it hosts. The same content fills the shell on the primary region and a nested outlet.
 */
function screenContent(scene: StageSceneApplication, screen: Screen): SceneElement[] {
    const template = templateFor(scene.screenTemplates, screen);
    const slotEntries = Object.entries(screen.slotContent);
    const screenForms = screen.forms.map(form => commandFormElement(screen.name, form));
    const templateSlots = template ? mergeSlots(template.content, screen.slotContent, { forms: screenForms }) : undefined;
    return template
        ? [externalComponent(`template-${screen.name}`, stageTemplateComponent, { arrangement: template.arrangement }, templateSlots)]
        : [...slotEntries.flatMap(([, elements]) => elements), ...screenForms];
}

/**
 * A screen as it appears inside an outlet of another screen: its content alone, without the application shell the
 * primary region wraps every screen in, with the same name resolution and data sources.
 */
function composeOutletScreen(scene: StageSceneApplication, screen: Screen, packageCatalog?: PackageCatalog): SceneElement {
    const content = screenContent(scene, screen);
    const element = content.length === 1 ? content[0] : { id: `outlet-${screen.name}`, children: content } as unknown as SceneElement;
    return resolveNames(bindDataSources(element), stageProfile(scene.uiProfiles), stageCatalogWith(packageCatalog));
}

/** Every destination the model authors - on actions, navigations, contributions and toolbar items - wherever it sits. */
function collectDestinations(scene: StageSceneApplication): DestinationReference[] {
    const found: DestinationReference[] = [];
    const visit = (value: unknown) => {
        if (value === null || typeof value !== 'object') return;
        if (Array.isArray(value)) {
            value.forEach(visit);
            return;
        }

        for (const [key, entry] of Object.entries(value as Record<string, unknown>)) {
            if (key === 'destination' && entry !== null && typeof entry === 'object' && !Array.isArray(entry)) found.push(entry as DestinationReference);
            visit(entry);
        }
    };

    for (const screen of scene.screens) {
        visit(screen.slotContent);
        visit(screen.contributions);
    }

    for (const template of [...scene.screenTemplates, ...(scene.dialogTemplates ?? [])]) visit(template.content);
    return found;
}

function commandFormElement(screenName: string, form: Form): SceneElement {
    return externalComponent(`form-${screenName}-${form.name}`, stageCommandFormComponent, {
        command: form.forCommand,
        label: form.name,
        generationMode: form.generationMode,
        layout: form.layout,
        fields: form.fields,
    });
}

function mergeSlots(...sources: (Record<string, SceneElement[]> | undefined)[]): Record<string, SceneElement[]> {
    const merged: Record<string, SceneElement[]> = {};
    for (const source of sources) {
        for (const [slot, elements] of Object.entries(source ?? {})) {
            merged[slot] = [...(merged[slot] ?? []), ...elements];
        }
    }

    return merged;
}

export interface AppProps {
    /** Components the host adds to the registry, beneath the Stage's own. */
    components?: ComponentRegistry;

    /**
     * What each package the host adds components for declares, by package name. A name an authored screen gives as
     * `Package.component`, or a bare name only such a package declares, resolves to the host's component through it.
     */
    packageCatalog?: PackageCatalog;
}

export function App({ components, packageCatalog }: AppProps = {}) {
    const source = useStageSource();
    const registry = useMemo(() => stageApplicationRegistry(components), [components]);
    const [scene, setScene] = useState<StageSceneApplication>();
    const [error, setError] = useState('');
    // Routes and strings are read alongside the scene, not after it, so a command is registered by the time it is first used.
    const routeState = useStageRouteState();
    const strings = useStrings();

    useEffect(() => {
        const abort = new AbortController();
        source.scene(abort.signal)
            .then(setScene)
            .catch(reason => {
                if (!abort.signal.aborted) setError(reason instanceof Error ? reason.message : String(reason));
            });
        return () => abort.abort();
    }, [source]);

    if (error) return <main className='stage-message'><h1>Unable to render this Stage</h1><p>{error}</p></main>;
    if (!scene) return <main className='stage-message'><h1>Preparing the Stage</h1></main>;
    return <StageApplication scene={scene} registry={registry} routeState={routeState} strings={strings} packageCatalog={packageCatalog} />;
}

interface StageApplicationProps {
    scene: StageSceneApplication;
    registry: ComponentRegistry;
    routeState: ReturnType<typeof useStageRouteState>;
    strings: ReturnType<typeof useStrings>;
    packageCatalog?: PackageCatalog;
}

/**
 * The application once its scene is known. Where the user is - the screen in the primary region, the screens in
 * nested outlets, the dialog on top, and the address that reaches it - is the Scene navigation host's: destinations
 * are resolved, placed in outlets and recorded in the URL fragment by Scene, the same way a generated or embedded
 * host does it. The Stage supplies what is its own: the shell, the data and the commands.
 */
function StageApplication({ scene, registry, routeState, strings, packageCatalog }: StageApplicationProps) {
    const routes = routeState.routes;
    const [activity, setActivity] = useState('');

    const history = useMemo(() => createHashSceneHistory(), []);
    const destinations = useMemo(() => collectDestinations(scene), [scene]);
    const outletScreens = useMemo(
        () => Object.fromEntries(scene.screens.map(candidate => [candidate.name, composeOutletScreen(scene, candidate, packageCatalog)])),
        [scene, packageCatalog]);
    const owners = useMemo(() => collectOutletOwners(outletScreens), [outletScreens]);
    const routeTable = useMemo(() => sceneRoutesFrom(scene.screens.map(candidate => candidate.name), destinations), [destinations, scene]);
    const navigation = useSceneHistoryNavigation(history, routeTable, scene.screens[0]?.name ?? '', hostBindingScope, owners, destinations);

    useEffect(() => setActivity(''), [navigation.currentScreen, navigation.currentDialog]);

    useEffect(() => {
        const command = (event: Event) => {
            const detail = (event as CustomEvent<SceneCommandDetail>).detail;
            setActivity(`The modeled command “${detail.command}” is ready for input.`);
        };
        globalThis.addEventListener('cratis.scene.command', command);
        return () => globalThis.removeEventListener('cratis.scene.command', command);
    }, []);

    const { navigate } = navigation;
    useEffect(() => {
        // What a component announces when it is outside a navigation host, or names only a screen: the same
        // destinations, executed by the host, once the model has them.
        const announced = (event: Event) => {
            const detail = (event as CustomEvent<SceneNavigateDetail>).detail;
            const destination: DestinationReference | undefined = detail.destination
                ?? (detail.dialog ? { kind: DestinationKind.Dialog, dialog: detail.dialog } : detail.targetScreen ? { screen: detail.targetScreen } : undefined);
            if (!destination) return;
            if (destination.dialog !== undefined) {
                if (scene.dialogTemplates?.some(candidate => candidate.name === destination.dialog)) navigate(destination);
                else setActivity(`The modeled dialog “${destination.dialog}” is not available in this Stage runtime.`);
            } else if (destination.screen !== undefined && !scene.screens.some(candidate => candidate.name === destination.screen)) {
                setActivity(`The modeled screen “${destination.screen}” is not available.`);
            } else {
                navigate(destination);
            }
        };

        globalThis.addEventListener('cratis.scene.navigate', announced);
        return () => globalThis.removeEventListener('cratis.scene.navigate', announced);
    }, [navigate, scene]);

    const screen = scene.screens.find(candidate => candidate.name === navigation.primaryScreen) ?? scene.screens[0];
    const element = useMemo(
        () => screen ? resolveStringsInElement(composeStageScreen(scene, screen, strings.locales, strings.locale, packageCatalog), strings.dictionary) : undefined,
        [scene, screen, strings.locales, strings.locale, strings.dictionary, packageCatalog],
    );

    if (!screen || !element) {
        return (
            <main className='stage-message'>
                <h1>Nothing to show yet</h1>
                <p>This model has no slice carrying a command or a read model, so there is no screen to render.</p>
            </main>
        );
    }

    return (
        <SceneNavigationProvider value={navigation}>
            <SceneOutletContext.Provider value={{ screens: outletScreens, registry }}>
                <LayoutConfigProvider>
                    <LayoutThemeProvider>
                        <ColorSchemeMirror />
                        <StageDataProvider routes={routes} routesReady={routeState.ready} parameters={navigation.currentParameters} locale={strings.locale} locales={strings.locales} screen={screen.name}>
                            <StageSceneView
                                activity={activity}
                                element={element}
                                registry={registry}
                                routes={routes}
                                routesReady={routeState.ready}
                                scene={scene}
                                setActivity={setActivity}
                                strings={strings} />
                        </StageDataProvider>
                    </LayoutThemeProvider>
                </LayoutConfigProvider>
            </SceneOutletContext.Provider>
        </SceneNavigationProvider>
    );
}

interface StageSceneViewProps {
    activity: string;
    element: SceneElement;
    registry: ComponentRegistry;
    routes: StageRoutes | undefined;
    routesReady: boolean;
    scene: StageSceneApplication;
    setActivity: (activity: string) => void;
    strings: ReturnType<typeof useStrings>;
}

function StageSceneView({ activity, element, registry, routes, routesReady, scene, setActivity, strings }: StageSceneViewProps) {
    const data = useStageData();
    const navigation = useSceneNavigation();
    const dialog = navigation.currentDialog ? scene.dialogTemplates?.find(candidate => candidate.name === navigation.currentDialog) : undefined;

    // Everything a document's interactions can do, pointed at the running application. The engine decides
    // what runs; this only says where a command goes and what a notification looks like.
    const dispatcher = createBrowserDispatcher({
        executeCommand: async (command, args): Promise<CommandOutcome> => {
            const route = routes?.commands[command];
            if (!route && !routesReady) {
                setActivity(`The modeled command “${command}” is waiting for Stage routes.`);
                return { isSuccess: false, validationErrors: [] };
            }

            if (!route) {
                setActivity(`The modeled command “${command}” is not registered by this Stage.`);
                return { isSuccess: false, validationErrors: [] };
            }

            const outcome = await executeStageCommand(command, route, args);
            if (outcome.messages.length > 0) setActivity(outcome.messages.join(' '));
            if (outcome.isSuccess) data.refreshAfterCommand();
            return { isSuccess: outcome.isSuccess, validationErrors: outcome.validationErrors, response: outcome.response };
        },
        // A navigation is a step the user can go back from, and its arguments are the screen's parameters - the
        // same address a row click or a deep link gives the screen.
        navigate: (screenName, args) => {
            if (scene.screens.some(candidate => candidate.name === screenName)) {
                navigateToScreen(screenName, screenArguments(args));
            } else {
                setActivity(`The modeled screen “${screenName}” is not available.`);
            }
        },
        navigateBack: () => globalThis.history?.back(),
        openDialog: async dialogTemplate => {
            const template = scene.dialogTemplates?.find(candidate => candidate.name === dialogTemplate);
            if (!template) {
                setActivity(`The modeled dialog “${dialogTemplate}” is not available in this Stage runtime.`);
                return { isConfirmed: false };
            }

            navigation.navigate({ kind: DestinationKind.Dialog, dialog: template.name });
            return { isConfirmed: true };
        },
        closeDialog: () => navigation.closeDialog(),
        confirm: async message => globalThis.confirm(message),
        setState: (target, value) => data.setState(target, value),
        notify: (level, message) => setActivity(`${level}: ${message}`),
        refreshQuery: async query => data.refreshQuery(query),
        raise: async trigger => setActivity(`The modeled trigger “${trigger}” is not available in this Stage runtime.`),
    });

    // A finding is what the engine could not do. Showing it is the whole difference between an interaction
    // that is broken and one that silently does nothing.
    const reportFindings = (findings: InteractionFinding[]) =>
        setActivity(findings.map(finding => finding.detail).join(' '));

    return (
        <StageChromeProvider state={{ locales: strings.locales, locale: strings.locale, setLocale: strings.setLocale, activity }}>
            <InteractionScope dispatcher={dispatcher} context={{ resolve: data.resolveBinding, localize: key => strings.dictionary[key] ?? key }} attachments={[]} onFindings={reportFindings}>
                <SceneElementView element={element} registry={registry} resolveBinding={data.resolveBinding} />
                {dialog && <StageDialog template={dialog} registry={registry} strings={strings} close={() => navigation.closeDialog()} />}
            </InteractionScope>
        </StageChromeProvider>
    );
}

interface StageDialogProps {
    template: DialogTemplate;
    registry: ComponentRegistry;
    strings: ReturnType<typeof useStrings>;
    close: () => void;
}

function StageDialog({ template, registry, strings, close }: StageDialogProps) {
    const data = useStageData();
    const profile = stageProfile(undefined);
    const content = resolveStringsInElement(
        resolveNames(externalComponent(`dialog-${template.name}`, stageTemplateComponent, { arrangement: template.arrangement }, template.content ?? {}), profile),
        strings.dictionary,
    );

    // A dialog is modal: it sits above the shell, it is reachable with the pointer wherever the page is scrolled to, and
    // Escape closes it - which, through the navigation host, goes back to where it was opened from.
    useEffect(() => {
        const escape = (event: KeyboardEvent) => {
            if (event.key === 'Escape') close();
        };
        globalThis.addEventListener('keydown', escape);
        return () => globalThis.removeEventListener('keydown', escape);
    }, [close]);

    return (
        <div className='stage-dialog-backdrop'>
            <section role='dialog' aria-modal='true' aria-label={template.displayName ?? template.name} className='stage-dialog'>
                <button type='button' onClick={close}>Close</button>
                <SceneElementView element={content} registry={registry} resolveBinding={data.resolveBinding} />
            </section>
        </div>
    );
}
