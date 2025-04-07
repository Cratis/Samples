// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { injectable } from 'tsyringe';
import { AddBookToInventory } from './AddBookToInventory';

@injectable()
export class AddBookViewModel {
    constructor(readonly command: AddBookToInventory) {
    }

    async register() {
        const result = await this.command.execute();
        this.command.clear();
    }
}
