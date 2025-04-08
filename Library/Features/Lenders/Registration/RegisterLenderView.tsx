// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog'
import { InputText } from 'primereact/inputtext';
import { AddAuthorViewModel } from './RegisterLenderViewModel';
import { withViewModel } from '@cratis/applications.react.mvvm';
import { FormElement } from 'Components/Common';
import * as piIcons from 'react-icons/pi';

export interface RegisterLenderViewProps {
    visible: boolean;
    onHide?: () => void;
}

export const RegisterLenderView = withViewModel<AddAuthorViewModel, RegisterLenderViewProps>(AddAuthorViewModel, ({ viewModel, props }) => {
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
            <FormElement icon={<piIcons.PiUser />}>
                <InputText placeholder="SocialSecurityNumber" value={viewModel.command.socialSecurityNumber} onChange={e => viewModel.command.socialSecurityNumber = e.target.value} />
            </FormElement>
            <FormElement icon={<piIcons.PiUser />}>
                <InputText placeholder="First Name" value={viewModel.command.firstName} onChange={e => viewModel.command.firstName = e.target.value} />
            </FormElement>
            <FormElement icon={<piIcons.PiUser />}>
                <InputText placeholder="Last Name" value={viewModel.command.lastName} onChange={e => viewModel.command.lastName = e.target.value} />
            </FormElement>
            <FormElement icon={<piIcons.PiUser />}>
                <InputText placeholder="Address" value={viewModel.command.address} onChange={e => viewModel.command.address = e.target.value} />
            </FormElement>
            <FormElement icon={<piIcons.PiUser />}>
                <InputText placeholder="Postal" value={viewModel.command.postalCode} onChange={e => viewModel.command.postalCode = e.target.value} />
            </FormElement>
            <FormElement icon={<piIcons.PiUser />}>
                <InputText placeholder="City" value={viewModel.command.city} onChange={e => viewModel.command.city = e.target.value} />
            </FormElement>
        </Dialog>
    )
});
