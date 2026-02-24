// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { DialogResult, useDialogContext } from '@cratis/arc.react/dialogs';
import { RegisterAuthor } from './RegisterAuthor';
import { CommandDialog } from '@cratis/components/CommandDialog';
import { InputTextField } from '@cratis/components/CommandForm';

export const AddAuthor = () => {
    const { closeDialog } = useDialogContext();

    return (
        <CommandDialog
            command={RegisterAuthor}
            visible={true}
            header="Register author"
            onConfirm={() => closeDialog(DialogResult.Ok)}
            onCancel={() => closeDialog(DialogResult.Cancelled)}>
            <InputTextField<RegisterAuthor> value={c => c.name} title="Name" placeholder="Name" />
        </CommandDialog>
    );
};
