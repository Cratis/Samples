// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useTheme } from './Utils/useTheme';
import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";
import { LayoutProvider } from './Layout/Default/context/LayoutContext';
import { DialogComponents } from '@cratis/applications.react.mvvm/dialogs';
import { ConfirmationDialog } from 'Components/Dialogs';
import { DefaultLayout } from './Layout/Default/DefaultLayout';
import { IMenuItemGroup } from './Layout/Default/Sidebar/MenuItem/MenuItem';
import { Home } from './Home';
import * as mdIcons from 'react-icons/md';
import { Authors } from './Features/Authors/Authors';

function App() {
    useTheme();

    const menuItems: IMenuItemGroup[] = [
        {
            items: [
                {
                    label: 'Authors',
                    url: '/authors',
                    icon: mdIcons.MdOutlinePeople,
                }
            ]
        }
    ];

    return (
        <LayoutProvider>
            <DialogComponents confirmation={ConfirmationDialog}>
                <BrowserRouter>
                    <Routes>
                        <Route element={<DefaultLayout menu={menuItems} />}>
                            <Route path='/' element={<Home />} />
                            <Route path='/authors' element={<Authors />} />
                        </Route>
                    </Routes>
                </BrowserRouter>
            </DialogComponents>
        </LayoutProvider>
    );
}

export default App;
