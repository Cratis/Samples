// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { injectable } from 'tsyringe';
import { Idea } from './Idea';

@injectable()
export class BoardViewModel {
    searchTerm = '';

    setSearchTerm(value: string) {
        this.searchTerm = value;
    }

    filter(ideas: Idea[]): Idea[] {
        const normalizedSearch = this.searchTerm.trim().toLowerCase();
        if (!normalizedSearch) {
            return ideas;
        }

        return ideas.filter(idea =>
            idea.title.toLowerCase().includes(normalizedSearch) ||
            idea.summary.toLowerCase().includes(normalizedSearch));
    }
}
