/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

/* eslint-disable sort-imports */
// eslint-disable-next-line header/header
import { QueryFor, QueryResultWithState, Sorting, Paging } from '@cratis/applications/queries';
import { useQuery, useQueryWithPaging, PerformQuery, SetSorting, SetPage, SetPageSize } from '@cratis/applications.react/queries';
import { Author } from './Author';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/authors/listing/get-all');

class GetAllSortBy {

    constructor(readonly query: GetAll) {
    }

}

class GetAllSortByWithoutQuery {

}

export class GetAll extends QueryFor<Author[]> {
    readonly route: string = '/api/authors/listing/get-all';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: Author[] = [];
    private readonly _sortBy: GetAllSortBy;
    private static readonly _sortBy: GetAllSortByWithoutQuery = new GetAllSortByWithoutQuery();

    constructor() {
        super(Author, true);
        this._sortBy = new GetAllSortBy(this);
    }

    get requiredRequestParameters(): string[] {
        return [
        ];
    }

    get sortBy(): GetAllSortBy {
        return this._sortBy;
    }

    static get sortBy(): GetAllSortByWithoutQuery {
        return this._sortBy;
    }

    static use(sorting?: Sorting): [QueryResultWithState<Author[]>, PerformQuery, SetSorting] {
        return useQuery<Author[], GetAll>(GetAll, undefined, sorting);
    }

    static useWithPaging(pageSize: number, sorting?: Sorting): [QueryResultWithState<Author[]>, PerformQuery, SetSorting, SetPage, SetPageSize] {
        return useQueryWithPaging<Author[], GetAll>(GetAll, new Paging(0, pageSize), undefined, sorting);
    }
}
