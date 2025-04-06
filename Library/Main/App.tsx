// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useTheme } from './Utils/useTheme';
import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";
import { LayoutProvider } from './Layout/Default/context/LayoutContext';
import { DialogComponents } from '@cratis/applications.react.mvvm/dialogs';
import { ConfirmationDialog } from 'Components/Dialogs';
import { Home } from './Home';
import { DefaultLayout } from './Layout/Default/DefaultLayout';
import { IMenuItemGroup } from './Layout/Default/Sidebar/MenuItem/MenuItem';
import * as mdIcons from 'react-icons/md';
import * as ioIcons from 'react-icons/io5';
import * as faIcons from 'react-icons/fa6';

function App() {
    useTheme();

    const menuItems: IMenuItemGroup[] = [
        {
            items: [
                { label: 'Authors', url: '/authors', icon: mdIcons.MdPeopleAlt },
                { label: 'Books', url: '/books', icon: mdIcons.MdBook },
                { label: 'Reservations', url: '/books/reservations', icon: ioIcons.IoBagCheckOutline },
                { label: 'Overdue books', url: '/books/overdue-books', icon: faIcons.FaRegCalendarXmark },
            ]
        }
    ];

    return (
        <LayoutProvider>
            <DialogComponents confirmation={ConfirmationDialog}>
                <BrowserRouter>
                    <Routes>
                        <Route path='/' element={<Navigate to={'/home'} />} />
                        <Route path='/home' element={<DefaultLayout menu={menuItems} />}>
                            <Route path={''} element={<Home />} />
                        </Route>
                    </Routes>
                </BrowserRouter>
            </DialogComponents>
        </LayoutProvider>
    );
}

export default App;
