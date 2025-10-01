// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Menubar } from 'primereact/menubar'
import { MenuItem } from 'primereact/menuitem';
import * as mdIcons from 'react-icons/md';
import { AddAuthor } from './Registration';
import { Listing } from './Listing/Listing';
import { DialogResult, useDialog } from '@cratis/applications.react/dialogs';
import { Page } from '../../Components/Common';

export const Authors = () => {
    const [AddAuthorDialog, showAddAuthorDialog] = useDialog(AddAuthor);

    const menuItems: MenuItem[] = [
        {
            label: 'Add Author',
            icon: mdIcons.MdPersonAdd,
            command: async () => {
                await showAddAuthorDialog();
            }
        }
    ];

    return (
        <Page title="Authors">
            <Menubar model={menuItems} />
            <Listing />

            <AddAuthorDialog />
        </Page>
    );
};
