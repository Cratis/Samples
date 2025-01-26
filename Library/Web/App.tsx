// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useTheme } from './Utils/useTheme';
import { BrowserRouter, Route, Routes } from "react-router-dom";
import { DialogComponents } from '@cratis/applications.react.mvvm/dialogs';
import { ConfirmationDialog } from 'Components/Dialogs';

function App() {
    useTheme();
    return (
        <DialogComponents confirmation={ConfirmationDialog}>
            <BrowserRouter>
                <Routes>
                    <Route path='/' element={<>Hello world</>} />
                </Routes>
            </BrowserRouter>
        </DialogComponents>
    );
}

export default App;
