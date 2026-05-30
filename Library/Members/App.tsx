// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { BrowserRouter, Route, Routes } from "react-router-dom";
import { DialogComponents } from '@cratis/arc.react/dialogs';
import { MembersHome } from './Home';
import { MembersList } from './Profiles/Listing/MembersList';
import { RegisterMember } from './Profiles/Registration/RegisterMemberPanel';

function App() {
    return (
        <DialogComponents confirmation={() => null} busyIndicator={() => null}>
            <BrowserRouter>
                <Routes>
                    <Route path='/' element={<MembersHome />} />
                    <Route path='/members' element={<MembersList />} />
                    <Route path='/members/register' element={<RegisterMember />} />
                </Routes>
            </BrowserRouter>
        </DialogComponents>
    );
}

export default App;
