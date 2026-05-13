// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { AddBook } from './Adding/AddBook';
import { Listing } from './Listing/Listing';
import { useDialog } from '@cratis/arc.react/dialogs';
import { Page } from '../../Components/Common';
import { Toolbar, ToolbarButton } from '@cratis/components/Toolbar';

export const Inventory = () => {
    const [AddBookDialog, showAddBookDialog] = useDialog(AddBook);

    return (
        <Page title="Books" panel>
            <Toolbar orientation="horizontal">
                <ToolbarButton icon="pi pi-book" text="Add book" tooltip="Add book" onClick={() => showAddBookDialog()} />
            </Toolbar>
            <Listing />

            <AddBookDialog />
        </Page>
    );
};
