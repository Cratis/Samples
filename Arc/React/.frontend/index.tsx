// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import '@cratis/components/tokens';
import '@cratis/components/styles';
import 'primeicons/primeicons.css';
import 'reflect-metadata';
import './index.css';
import { Bindings } from '@cratis/arc.react.mvvm';
import { configure as configureMobx } from 'mobx';
import React from 'react';
import ReactDOM from 'react-dom/client';
import { App } from './App';

Bindings.initialize();
configureMobx({ enforceActions: 'never' });

ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <App />
    </React.StrictMode>
);
