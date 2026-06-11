// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { BrowserRouter, Route, Routes } from "react-router-dom";
import { DialogComponents } from '@cratis/arc.react/dialogs';
import { MembersHome } from '../Home';
import { MyProfile } from '../Profiles/MyProfile';

function App() {
    return (
        <DialogComponents confirmation={() => null} busyIndicator={() => null}>
            <BrowserRouter>
                <Routes>
                    <Route path='/' element={<MembersHome />} />
                    <Route path='/profile' element={<MyProfile />} />
                </Routes>
            </BrowserRouter>
        </DialogComponents>
    );
}

export default App;
