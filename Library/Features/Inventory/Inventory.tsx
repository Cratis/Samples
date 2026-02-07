// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Menubar } from 'primereact/menubar'
import { MenuItem } from 'primereact/menuitem';
import * as mdIcons from 'react-icons/md';
import { AddBook } from './Adding/AddBook';
import { Listing } from './Listing/Listing';
import { DialogResult, useDialog } from '@cratis/arc.react/dialogs';
import { Page } from '../../Components/Common';

export const Inventory = () => {
    const [AddBookDialog, showAddBookDialog] = useDialog(AddBook);

    const menuItems: MenuItem[] = [
        {
            label: 'Add book',
            icon: mdIcons.MdPersonAdd,
            command: async () => {
                await showAddBookDialog();
            }
        }
    ];

    return (
        <Page title="Books">
            <Menubar model={menuItems} />
            <Listing />

            <AddBookDialog />
        </Page>
    );
};
