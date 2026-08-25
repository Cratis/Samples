// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Arc } from '@cratis/arc.react';
import { DialogComponents } from '@cratis/arc.react/dialogs';
import { CratisComponentsProvider } from '@cratis/components/Common';
import { styledMode } from '@cratis/components/styled';
import { BusyIndicatorDialog, ConfirmationDialog } from '@cratis/components/Dialogs';
import { Board } from '../Ideas/Board/Board';

export const App = () => (
    <CratisComponentsProvider value={{ ripple: true, ...styledMode() }}>
        <Arc>
            <DialogComponents confirmation={ConfirmationDialog} busyIndicator={BusyIndicatorDialog}>
                <Board />
            </DialogComponents>
        </Arc>
    </CratisComponentsProvider>
);
