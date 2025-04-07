// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog'
import { InputText } from 'primereact/inputtext';
import { AddAuthorViewModel } from './AddAuthorViewModel';
import { withViewModel } from '@cratis/applications.react.mvvm';

export interface AddAuthorProps {
    visible: boolean;
    onHide?: () => void;
}

export const AddAuthor = withViewModel<AddAuthorViewModel, AddAuthorProps>(AddAuthorViewModel, ({ viewModel, props }) => {
    const ok = async () => {
        await viewModel.register();
        props.onHide?.();
    };

    const cancel = () => {
        props.onHide?.();
    };

    const footerContent = (
        <div className="flex flex-wrap gap-3">
            <Button label="Ok" icon="pi pi-check" onClick={ok} autoFocus />
            <Button label="Cancel" icon="pi pi-times" severity='secondary' onClick={cancel} />
        </div>
    );

    return (
        <Dialog header='Add author' visible={props.visible} style={{ width: '20vw' }} modal onHide={cancel} footer={footerContent}>
            <div className="card flex flex-column md:flex-row gap-3">
                <div className="p-inputgroup flex-1">
                    <span className="p-inputgroup-addon">
                        <i className="pi pi-user"></i>
                    </span>
                    <InputText placeholder="Name" value={viewModel.command.name} onChange={e => viewModel.command.name = e.target.value} />
                </div>
            </div>
        </Dialog>
    )
});
