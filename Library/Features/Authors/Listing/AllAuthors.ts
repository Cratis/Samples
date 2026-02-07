/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

/* eslint-disable sort-imports */
// eslint-disable-next-line header/header
import { ObservableQueryFor, QueryResultWithState, Sorting, Paging } from '@cratis/arc/queries';
import { useObservableQuery, useObservableQueryWithPaging, SetSorting, SetPage, SetPageSize } from '@cratis/arc.react/queries';
import { Author } from './Author';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/authors/listing/all-authors');

class AllAuthorsSortBy {

    constructor(readonly query: AllAuthors) {
    }

}

class AllAuthorsSortByWithoutQuery {

}

export class AllAuthors extends ObservableQueryFor<Author[]> {
    readonly route: string = '/api/authors/listing/all-authors';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: Author[] = [];
    private readonly _sortBy: AllAuthorsSortBy;
    private static readonly _sortBy: AllAuthorsSortByWithoutQuery = new AllAuthorsSortByWithoutQuery();

    constructor() {
        super(Author, true);
        this._sortBy = new AllAuthorsSortBy(this);
    }

    get requiredRequestParameters(): string[] {
        return [
        ];
    }

    get sortBy(): AllAuthorsSortBy {
        return this._sortBy;
    }

    static get sortBy(): AllAuthorsSortByWithoutQuery {
        return this._sortBy;
    }

    static use(sorting?: Sorting): [QueryResultWithState<Author[]>, SetSorting] {
        return useObservableQuery<Author[], AllAuthors>(AllAuthors, undefined, sorting);
    }

    static useWithPaging(pageSize: number, sorting?: Sorting): [QueryResultWithState<Author[]>, SetSorting, SetPage, SetPageSize] {
        return useObservableQueryWithPaging<Author[], AllAuthors>(AllAuthors, new Paging(0, pageSize), undefined, sorting);
    }
}
