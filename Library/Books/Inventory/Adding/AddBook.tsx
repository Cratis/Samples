// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog'
import { InputText } from 'primereact/inputtext';
import { AddBookViewModel } from './AddBookViewModel';
import { withViewModel } from '@cratis/applications.react.mvvm';
import { FormElement } from 'Components/Common';
import * as piIcons from 'react-icons/pi';
import { Dropdown } from 'primereact/dropdown';

export interface AddBookProps {
    visible: boolean;
    onHide?: () => void;
}

export const AddBook = withViewModel<AddBookViewModel, AddBookProps>(AddBookViewModel, ({ viewModel, props }) => {
    const ok = async () => {
        await viewModel.add();
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
        <Dialog header='Add book' visible={props.visible} style={{ width: '20vw' }} modal onHide={cancel} footer={footerContent}>
            <FormElement icon={<piIcons.PiBarcode />}>
                <InputText placeholder="ISBN" value={viewModel.command.ISBN} onChange={e => viewModel.command.ISBN = e.target.value} />
            </FormElement>
            <FormElement icon={<piIcons.PiUser />}>
                <Dropdown
                    value={viewModel.selectedAuthor}
                    onChange={(e) => viewModel.selectedAuthor = e.value}
                    options={viewModel.authors}
                    optionLabel="name"
                    placeholder="Select an author" />
            </FormElement>
            <FormElement icon={<piIcons.PiSubtitles />}>
                <InputText placeholder="Title" value={viewModel.command.title} onChange={e => viewModel.command.title = e.target.value} />
            </FormElement>
        </Dialog>
    )
});
