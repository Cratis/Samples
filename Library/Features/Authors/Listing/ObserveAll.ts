/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

/* eslint-disable sort-imports */
// eslint-disable-next-line header/header
import { ObservableQueryFor, QueryResultWithState, Sorting, Paging } from '@cratis/applications/queries';
import { useObservableQuery, useObservableQueryWithPaging, SetSorting, SetPage, SetPageSize } from '@cratis/applications.react/queries';
import { Author } from './Author';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/authors/listing/observe-all');

class ObserveAllSortBy {

    constructor(readonly query: ObserveAll) {
    }

}

class ObserveAllSortByWithoutQuery {

}

export class ObserveAll extends ObservableQueryFor<Author[]> {
    readonly route: string = '/api/authors/listing/observe-all';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: Author[] = [];
    private readonly _sortBy: ObserveAllSortBy;
    private static readonly _sortBy: ObserveAllSortByWithoutQuery = new ObserveAllSortByWithoutQuery();

    constructor() {
        super(Author, true);
        this._sortBy = new ObserveAllSortBy(this);
    }

    get requiredRequestParameters(): string[] {
        return [
        ];
    }

    get sortBy(): ObserveAllSortBy {
        return this._sortBy;
    }

    static get sortBy(): ObserveAllSortByWithoutQuery {
        return this._sortBy;
    }

    static use(sorting?: Sorting): [QueryResultWithState<Author[]>, SetSorting] {
        return useObservableQuery<Author[], ObserveAll>(ObserveAll, undefined, sorting);
    }

    static useWithPaging(pageSize: number, sorting?: Sorting): [QueryResultWithState<Author[]>, SetSorting, SetPage, SetPageSize] {
        return useObservableQueryWithPaging<Author[], ObserveAll>(ObserveAll, new Paging(0, pageSize), undefined, sorting);
    }
}
