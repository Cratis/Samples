// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Column } from 'primereact/column';
import { DataPage, MenuItem } from '../Components/DataPage';
import { AllAuthors } from './Listing/AllAuthors';
import * as faIcons from 'react-icons/fa';
import { AddAuthor } from './Registration/AddAuthor';
import { useState } from 'react';

export const Authors = () => {
    const [addAuthorDialogVisible, setAddAuthorDialogVisible] = useState(false);

    return (
        <>
            <DataPage
                title='Authors'
                query={AllAuthors}
                dataKey='id'
                emptyMessage='No authors found'>

                <DataPage.MenuItems>
                    <MenuItem id='add' label='Add Author' icon={faIcons.FaPlus} command={() => setAddAuthorDialogVisible(true)} />
                </DataPage.MenuItems>

                <DataPage.Columns>
                    <Column field='id' header='Identifier' />
                </DataPage.Columns>
            </DataPage>
            <AddAuthor visible={addAuthorDialogVisible} onHide={() => setAddAuthorDialogVisible(false)} />
        </>
    );
};
