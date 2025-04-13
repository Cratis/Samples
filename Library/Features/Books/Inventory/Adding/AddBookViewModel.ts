// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { injectable } from 'tsyringe';
import { AddBookToInventory } from './AddBookToInventory';
import { AllAuthors, Author } from '../../../Authors/Listing';

@injectable()
export class AddBookViewModel {
    constructor(
        readonly command: AddBookToInventory,
        authors: AllAuthors) {
            authors.perform().then(result => this.authors = result.data)
    }

    selectedAuthor?: Author;
    authors: Author[] = [];

    async add() {
        this.command.authorId = this.selectedAuthor?.id!;
        const result = await this.command.execute();
        this.command.clear();
        this.selectedAuthor = null!;
    }
}
