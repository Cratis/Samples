/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

/* eslint-disable sort-imports */
// eslint-disable-next-line header/header
import { QueryFor, QueryResultWithState, Sorting, SortingActions, SortingActionsForQuery, Paging } from '@cratis/applications/queries';
import { useQuery, useQueryWithPaging, PerformQuery, SetSorting, SetPage, SetPageSize } from '@cratis/applications.react/queries';
import { Book } from './Book';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/books');

class GetAllSortBy {
    private _id: SortingActionsForQuery<Book[]>;
    private _title: SortingActionsForQuery<Book[]>;

    constructor(readonly query: GetAll) {
        this._id = new SortingActionsForQuery<Book[]>('id', query);
        this._title = new SortingActionsForQuery<Book[]>('title', query);
    }

    get id(): SortingActionsForQuery<Book[]> {
        return this._id;
    }
    get title(): SortingActionsForQuery<Book[]> {
        return this._title;
    }
}

class GetAllSortByWithoutQuery {
    private _id: SortingActions  = new SortingActions('id');
    private _title: SortingActions  = new SortingActions('title');

    get id(): SortingActions {
        return this._id;
    }
    get title(): SortingActions {
        return this._title;
    }
}

export class GetAll extends QueryFor<Book[]> {
    readonly route: string = '/api/books';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: Book[] = [];
    private readonly _sortBy: GetAllSortBy;
    private static readonly _sortBy: GetAllSortByWithoutQuery = new GetAllSortByWithoutQuery();

    constructor() {
        super(Book, true);
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

    static use(sorting?: Sorting): [QueryResultWithState<Book[]>, PerformQuery, SetSorting] {
        return useQuery<Book[], GetAll>(GetAll, undefined, sorting);
    }

    static useWithPaging(pageSize: number, sorting?: Sorting): [QueryResultWithState<Book[]>, PerformQuery, SetSorting, SetPage, SetPageSize] {
        return useQueryWithPaging<Book[], GetAll>(GetAll, new Paging(0, pageSize), undefined, sorting);
    }
}
