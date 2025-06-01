// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Menubar } from 'primereact/menubar'
import { MenuItem } from 'primereact/menuitem';
import * as mdIcons from 'react-icons/md';
import { AddAuthor } from './Registration/AddAuthor';
import { Listing } from './Listing/Listing';
import { useDialog } from '@cratis/applications.react/dialogs';

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
        <div>
            <Menubar model={menuItems} />
            <Listing />

            <AddAuthorDialog />
        </div>
    );
};
