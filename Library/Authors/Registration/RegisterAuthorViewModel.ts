// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { injectable } from 'tsyringe';
import { RegisterAuthor } from './RegisterAuthor';
import { Guid } from '@cratis/fundamentals';

@injectable()
export class AddAuthorViewModel {
    constructor(readonly command: RegisterAuthor) {
    }

    async register() {
        const result = await this.command.execute();
        this.command.clear();
    }
}
