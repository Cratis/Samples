// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import 'chai';
import { beforeEach, describe, it } from 'vitest';
import { BoardViewModel } from '../../BoardViewModel';
import { Idea } from '../../Idea';

describe('when filtering ideas by title or summary', () => {
    let viewModel: BoardViewModel;
    let ideas: Idea[];

    beforeEach(() => {
        viewModel = new BoardViewModel();
        ideas = [
            Object.assign(new Idea(), { title: 'Shorten setup', summary: 'Make the first run obvious.' }),
            Object.assign(new Idea(), { title: 'Polish empty states', summary: 'Help new users find their next action.' }),
        ];
        viewModel.setSearchTerm('FIRST RUN');
    });

    it('should match without regard to casing', () => {
        viewModel.filter(ideas).should.have.lengthOf(1);
    });

    it('should match text carried by the summary', () => {
        const matchingIdeas = viewModel.filter(ideas);
        matchingIdeas[0].title.should.equal('Shorten setup');
    });
});
