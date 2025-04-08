// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { injectable } from 'tsyringe';
import { RegisterLender } from './RegisterLender';

@injectable()
export class AddAuthorViewModel {
    constructor(readonly command: RegisterLender) {
    }

    async register() {
        const result = await this.command.execute();
        this.command.clear();
    }
}
