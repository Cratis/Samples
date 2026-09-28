// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { DialogComponents } from '@cratis/arc.react/dialogs';
import { BusyIndicatorDialog, ConfirmationDialog } from '@cratis/components/Dialogs';
import { Authors } from './Authors/Authors';

function App() {
    return (
        <DialogComponents confirmation={ConfirmationDialog} busyIndicator={BusyIndicatorDialog}>
            <Authors />
        </DialogComponents>
    );
}

export default App;
