// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { AddBook } from './Adding/AddBook';
import { Listing } from './Listing/Listing';
import { useDialog } from '@cratis/arc.react/dialogs';
import { Button, Page } from '../Components/Common';
import { FaBook } from 'react-icons/fa6';

export const Inventory = () => {
    const [AddBookDialog, showAddBookDialog] = useDialog(AddBook);

    return (
        <Page title="Books" panel>
            <div className="flex gap-2 p-3">
                <Button label="Add book" icon={<FaBook aria-hidden="true" />} onClick={() => showAddBookDialog()} />
            </div>
            <Listing />

            <AddBookDialog />
        </Page>
    );
};
