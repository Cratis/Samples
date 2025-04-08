// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Column } from 'primereact/column';
import { DataPage, MenuItem } from '../../Components/DataPage';
import { ObserveAllBooks } from './Listing';
import * as faIcons from 'react-icons/fa';
import { AddBook } from './Adding/AddBook';
import { useState } from 'react';

export const Inventory = () => {
    const [addBookDialogVisible, setAddAuthorDialogVisible] = useState(false);

    return (
        <>
            <DataPage
                title='Books Inventory'
                query={ObserveAllBooks}
                dataKey='id'
                emptyMessage='No books found'>

                <DataPage.MenuItems>
                    <MenuItem id='add' label='Add Book' icon={faIcons.FaPlus} command={() => setAddAuthorDialogVisible(true)} />
                </DataPage.MenuItems>

                <DataPage.Columns>
                    <Column field='title' header='Title' />
                </DataPage.Columns>
            </DataPage>
            <AddBook visible={addBookDialogVisible} onHide={() => setAddAuthorDialogVisible(false)} />
        </>
    );
};
