// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { RegisterAuthor } from './RegisterAuthor';
import { CommandDialog } from '@cratis/components/CommandDialog';
import { InputTextField } from '@cratis/components/CommandForm';

export const AddAuthor = () => {
    return (
        <CommandDialog
            command={RegisterAuthor}
            title="Register author">
            <InputTextField<RegisterAuthor> value={c => c.name} title="Name" placeholder="Name" />
        </CommandDialog>
    );
};
