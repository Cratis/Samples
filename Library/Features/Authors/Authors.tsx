// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { AddAuthor } from './Registration';
import { Listing } from './Listing/Listing';
import { useDialog } from '@cratis/arc.react/dialogs';
import { Page } from '../../Components/Common';
import { Toolbar, ToolbarButton } from '@cratis/components/Toolbar';

export const Authors = () => {
    const [AddAuthorDialog, showAddAuthorDialog] = useDialog(AddAuthor);

    return (
        <Page title="Authors" panel>
            <Toolbar orientation="horizontal">
                <ToolbarButton icon="pi pi-user" text="Add Author" tooltip="Add Author" onClick={() => showAddAuthorDialog()} />
            </Toolbar>
            <Listing />

            <AddAuthorDialog />
        </Page>
    );
};
