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
import { Authors } from './Authors/Authors';
import * as mdIcons from 'react-icons/md';
import * as ioIcons from 'react-icons/io5';
import * as faIcons from 'react-icons/fa6';
import { Lenders } from './Lenders/Lenders';
import { Books } from './Books/Books';

function App() {
    useTheme();

    const menuItems: IMenuItemGroup[] = [
        {
            items: [
                { label: 'Authors', url: '/authors', icon: mdIcons.MdPeopleAlt },
                { label: 'Books', url: '/books/inventory', icon: mdIcons.MdBook },
                { label: 'Reservations', url: '/books/reservations', icon: ioIcons.IoBagCheckOutline },
                { label: 'Overdue books', url: '/books/overdue', icon: faIcons.FaRegCalendarXmark },
                { label: 'Lenders', url: '/lenders', icon: mdIcons.MdPeopleAlt },
            ]
        }
    ];

    return (
        <LayoutProvider>
            <DialogComponents confirmation={ConfirmationDialog}>
                <BrowserRouter>
                    <Routes>
                        <Route element={<DefaultLayout menu={menuItems} />}>
                            <Route path='/' element={<Navigate to={'/home'} />} />
                            <Route path='/home' >
                                <Route path={''} element={<Home />} />
                            </Route>

                            <Route path='/authors' element={<Authors />}/>
                            <Route path='/books/*' element={<Books />}/>
                            <Route path='/lenders' element={<Lenders />}/>
                        </Route>
                    </Routes>
                </BrowserRouter>
            </DialogComponents>
        </LayoutProvider>
    );
}

export default App;
