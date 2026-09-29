// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import 'reflect-metadata';
import { CratisComponentsProvider } from '@cratis/components';
import ReactDOM from 'react-dom/client';
import './index.css';
import React from 'react';
import App from "./App";
import { configure as configureMobx } from 'mobx';
import { Bindings } from '../Bindings';

Bindings.initialize();

configureMobx({
    enforceActions: 'never'
});

ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <CratisComponentsProvider>
            <App />
        </CratisComponentsProvider>
    </React.StrictMode>
);
