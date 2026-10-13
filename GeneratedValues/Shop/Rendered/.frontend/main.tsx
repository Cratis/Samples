import 'reflect-metadata';
import './index.css';
import 'primeicons/primeicons.css';
import '@cratis/scene.primereact/primeReactTheme.css';
import React from 'react';
import ReactDOM from 'react-dom/client';
import { Arc } from '@cratis/arc.react';
import { CratisComponentsProvider } from '@cratis/components';
import { PrimeReactProvider } from '@primereact/core/config';
// The runtime entry: the components, their descriptors and the manifest, without the designers, property
// editors and design-time actions a running application never loads.
import { cratisComponentsRuntimePackage } from '@cratis/scene.components/runtime';
import { App } from './stage/App';
import { StageSourceProvider, staticStageSource } from './stage/stageSource';
import { stageTheme } from './stage/stageTheme';
import { stage } from '../src/stage';

// The screens are composed, not written here: `src/stage` holds the Scene this application was rendered
// with, the routes its generated proxies registered and its strings, and `./stage` is the same runtime a
// live Stage renders that Scene with. Editing this file to add a screen would put it outside the
// composition the backend was generated from.
const source = staticStageSource(stage);

// PrimeReact 11 verifies a license key at runtime and otherwise shows its unlicensed presentation. The key
// comes from PRIMEUI_LICENSE (or STAGE_PRIMEUI_LICENSE) when the application is built - see vite.config.ts.
const primeUiLicense = import.meta.env.STAGE_PRIMEUI_LICENSE || '';

// A lazy CSS import orders user-owned tokens after the managed styles, before React renders.
const customStyles = import.meta.glob('../Customizations/styles.css');
await Object.values(customStyles)[0]?.();

ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <PrimeReactProvider license={primeUiLicense} theme={stageTheme}>
            <CratisComponentsProvider>
                <Arc>
                    <StageSourceProvider source={source}>
                        <App components={cratisComponentsRuntimePackage.components} />
                    </StageSourceProvider>
                </Arc>
            </CratisComponentsProvider>
        </PrimeReactProvider>
    </React.StrictMode>
);
