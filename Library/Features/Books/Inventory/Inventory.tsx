// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Column } from 'primereact/column';
import { DataPage, MenuItem } from '../../../Components/DataPage';
import { Book, ObserveAllBooks } from './Listing';
import * as faIcons from 'react-icons/fa';
import { AddBook } from './Adding/AddBook';
import { useState } from 'react';

export const Inventory = () => {
    const [addBookDialogVisible, setAddAuthorDialogVisible] = useState(false);
    const [selectedBook, setSelectedBook] = useState<Book | undefined>();

    return (
        <>
            <DataPage
                title='Books Inventory'
                query={ObserveAllBooks}
                dataKey='id'
                selection={selectedBook}
                onSelectionChange={(e) => setSelectedBook(e.value)}
                emptyMessage='No books found'>

                <DataPage.MenuItems>
                    <MenuItem id='add' label='Add Book' icon={faIcons.FaPlus} command={() => setAddAuthorDialogVisible(true)} />
                    <MenuItem id='reserve' label='Reserve Book' disableOnUnselected icon={faIcons.FaCalendarCheck} command={() => {}} />
                    <MenuItem id='lend' label='Lend Book' disableOnUnselected icon={faIcons.FaCalendarPlus} command={() => {}} />
                </DataPage.MenuItems>

                <DataPage.Columns>
                    <Column field='title' header='Title' />
                    <Column field='stockCount' header='Total stock count' />
                </DataPage.Columns>
            </DataPage>
            <AddBook visible={addBookDialogVisible} onHide={() => setAddAuthorDialogVisible(false)} />
        </>
    );
};
