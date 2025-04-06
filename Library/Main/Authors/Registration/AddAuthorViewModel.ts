// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { injectable } from 'tsyringe';
import { RegisterAuthor } from './RegisterAuthor';
import { Guid } from '@cratis/fundamentals';

@injectable()
export class AddAuthorViewModel {
    constructor(readonly command: RegisterAuthor) {
        this.command.authorId = Guid.create();
    }

    async register() {
        await this.command.execute();
    }
}
