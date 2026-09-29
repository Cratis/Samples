// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { AddAuthor } from './Registration';
import { Listing } from './Listing/Listing';
import { useDialog } from '@cratis/arc.react/dialogs';
import { Button, Page } from '../Components/Common';
import { FaUser } from 'react-icons/fa6';

export const Authors = () => {
    const [AddAuthorDialog, showAddAuthorDialog] = useDialog(AddAuthor);

    return (
        <Page title="Authors" panel>
            <div className="flex gap-2 p-3">
                <Button label="Add Author" icon={<FaUser aria-hidden="true" />} onClick={() => showAddAuthorDialog()} />
            </div>
            <Listing />

            <AddAuthorDialog />
        </Page>
    );
};
