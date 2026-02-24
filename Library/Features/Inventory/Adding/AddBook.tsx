// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { DialogResult, useDialogContext } from '@cratis/arc.react/dialogs';
import { AddBookTitleToInventory } from './AddBookTitleToInventory';
import { AllAuthors } from '../../Authors/Listing/AllAuthors';
import { CommandDialog } from '@cratis/components/CommandDialog';
import { InputTextField, NumberField, DropdownField } from '@cratis/components/CommandForm';

export const AddBook = () => {
    const { closeDialog } = useDialogContext();
    const [authors] = AllAuthors.use();

    return (
        <CommandDialog
            command={AddBookTitleToInventory}
            visible={true}
            header="Add book"
            onConfirm={() => closeDialog(DialogResult.Ok)}
            onCancel={() => closeDialog(DialogResult.Cancelled)}>
            <InputTextField<AddBookTitleToInventory> value={c => c.title} title="Title" placeholder="Title" />
            <InputTextField<AddBookTitleToInventory> value={c => c.ISBN} title="ISBN" placeholder="ISBN" />
            <NumberField<AddBookTitleToInventory> value={c => c.count} title="Count" placeholder="Count" />
            <DropdownField<AddBookTitleToInventory>
                value={c => c.authorId}
                title="Author"
                options={authors.data}
                optionValue="id"
                optionLabel="name"
                placeholder="Select an author"
            />
        </CommandDialog>
    );
};
