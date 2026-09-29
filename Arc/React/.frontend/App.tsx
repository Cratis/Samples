// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Arc } from '@cratis/arc.react';
import { DialogComponents } from '@cratis/arc.react/dialogs';
import { CratisComponentsProvider } from '@cratis/components';
import { BusyIndicatorDialog, ConfirmationDialog } from '@cratis/components/Dialogs';
import { BrowserRouter } from 'react-router-dom';
import { Board } from '../Ideas/Board/Board';

// withViewModel reads route and query parameters through React Router, so the view needs a router above it.
export const App = () => (
    <CratisComponentsProvider>
        <Arc>
            <DialogComponents confirmation={ConfirmationDialog} busyIndicator={BusyIndicatorDialog}>
                <BrowserRouter>
                    <Board />
                </BrowserRouter>
            </DialogComponents>
        </Arc>
    </CratisComponentsProvider>
);
