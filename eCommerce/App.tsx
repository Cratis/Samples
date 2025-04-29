// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useTheme } from './Utils/useTheme';
import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";
import { LayoutProvider } from './Layout/Default/context/LayoutContext';
import { DialogComponents } from '@cratis/applications.react.mvvm/dialogs';
import { ConfirmationDialog } from 'Components/Dialogs';
import { Catalog } from './Catalog';
import { DefaultLayout } from './Layout/Default/DefaultLayout';
import { IMenuItemGroup } from './Layout/Default/Sidebar/MenuItem/MenuItem';
import * as FaMdb from 'react-icons/fa6';

function App() {
    useTheme();

    const menuItems: IMenuItemGroup[] = [
        {
            items: [
                {
                    label: 'Catalog',
                    url: '/catalog',
                    icon: FaMdb.FaHouse
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
                            <Route path='/' element={<Navigate to={'/catalog'} />} />
                            <Route path='/catalog' >
                                <Route path={''} element={<Catalog />} />
                            </Route>
                        </Route>
                    </Routes>
                </BrowserRouter>
            </DialogComponents>
        </LayoutProvider>
    );
}

export default App;
