// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';
import { DialogResult, useDialogContext } from '@cratis/applications.react/dialogs';
import { InputText } from 'primereact/inputtext';
import { RegisterAuthor } from './RegisterAuthor';
import { useState } from 'react';

export const AddAuthor = () => {
    const [name, setName] = useState<string>('');
    const { closeDialog } = useDialogContext();
    const [registerAuthor] = RegisterAuthor.use();

    const handleAdd = async () => {
        registerAuthor.name = name;
        await registerAuthor.execute();
        closeDialog(DialogResult.Ok);
    };

    return (
        <Dialog header="Register author" visible={true} style={{ width: '50vw' }} onHide={() => closeDialog(DialogResult.Cancelled)}>
            <div className="card flex flex-column md:flex-row gap-3">
                <div className="p-inputgroup flex-1">
                    <span className="p-inputgroup-addon">
                        <i className="pi pi-user"></i>
                    </span>
                    <InputText placeholder="Name" value={name} onChange={e => setName(e.target.value)} />
                </div>
            </div>

            <div className="card flex flex-wrap justify-content-center gap-3 mt-8">
                <Button label="Add" icon="pi pi-check" onClick={handleAdd} autoFocus />
                <Button label="Cancel" icon="pi pi-times" severity='secondary' onClick={() => closeDialog(DialogResult.Cancelled)} />
            </div>
        </Dialog>
    );
};
