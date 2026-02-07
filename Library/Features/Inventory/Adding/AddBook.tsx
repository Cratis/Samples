// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';
import { DialogResult, useDialogContext } from '@cratis/arc.react/dialogs';
import { InputText } from 'primereact/inputtext';
import { AddBookTitleToInventory } from './AddBookTitleToInventory';
import { useState } from 'react';
import { Guid } from '@cratis/fundamentals';
import { AllAuthors } from '../../Authors/Listing/AllAuthors';

export const AddBook = () => {
    const [title, setTitle] = useState('');
    const [ISBN, setISBN] = useState('');
    const [count, setCount] = useState('');
    const [authorId, setAuthorId] = useState(Guid.empty);
    const { closeDialog } = useDialogContext();
    const [command] = AddBookTitleToInventory.use();
    const [authors]  = AllAuthors.use();

    const handleAdd = async () => {
        command.title = title;
        command.ISBN = ISBN;
        command.authorId = authors.data[0]?.id || Guid.empty;
        command.title = title;
        await command.execute();
        closeDialog(DialogResult.Ok);
    };

    return (
        <Dialog header="Add author" visible={true} style={{ width: '50vw' }} onHide={() => closeDialog(DialogResult.Cancelled)}>
            <div className="card flex flex-column md:flex-row gap-3">
                <div className="p-inputgroup flex-1">
                    <span className="p-inputgroup-addon">
                        <i className="pi pi-user"></i>
                    </span>
                    <InputText placeholder="Title" value={title} onChange={e => setTitle(e.target.value)} />
                </div>
            </div>

            <div className="card flex flex-column md:flex-row gap-3">
                <div className="p-inputgroup flex-1">
                    <span className="p-inputgroup-addon">
                        <i className="pi pi-user"></i>
                    </span>
                    <InputText placeholder="ISBN" value={ISBN} onChange={e => setISBN(e.target.value)} />
                </div>
            </div>

            <div className="card flex flex-column md:flex-row gap-3">
                <div className="p-inputgroup flex-1">
                    <span className="p-inputgroup-addon">
                        <i className="pi pi-user"></i>
                    </span>
                    <InputText placeholder="Count" value={count} onChange={e => setCount(e.target.value)} />
                </div>
            </div>

            <div className="card flex flex-wrap justify-content-center gap-3 mt-8">
                <Button label="Add" icon="pi pi-check" onClick={handleAdd} autoFocus />
                <Button label="Cancel" icon="pi pi-times" severity='secondary' onClick={() => closeDialog(DialogResult.Cancelled)} />
            </div>
        </Dialog>
    );
};
