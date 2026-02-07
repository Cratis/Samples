/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

/* eslint-disable sort-imports */
// eslint-disable-next-line header/header
import { ObservableQueryFor, QueryResultWithState, Sorting, SortingActions, SortingActionsForObservableQuery, Paging } from '@cratis/arc/queries';
import { useObservableQuery, useObservableQueryWithPaging, SetSorting, SetPage, SetPageSize } from '@cratis/arc.react/queries';
import { Book } from './Book';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/books/observe');

class ObserveAllSortBy {
    private _id: SortingActionsForObservableQuery<Book[]>;
    private _title: SortingActionsForObservableQuery<Book[]>;

    constructor(readonly query: ObserveAll) {
        this._id = new SortingActionsForObservableQuery<Book[]>('id', query);
        this._title = new SortingActionsForObservableQuery<Book[]>('title', query);
    }

    get id(): SortingActionsForObservableQuery<Book[]> {
        return this._id;
    }
    get title(): SortingActionsForObservableQuery<Book[]> {
        return this._title;
    }
}

class ObserveAllSortByWithoutQuery {
    private _id: SortingActions  = new SortingActions('id');
    private _title: SortingActions  = new SortingActions('title');

    get id(): SortingActions {
        return this._id;
    }
    get title(): SortingActions {
        return this._title;
    }
}

export class ObserveAll extends ObservableQueryFor<Book[]> {
    readonly route: string = '/api/books/observe';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: Book[] = [];
    private readonly _sortBy: ObserveAllSortBy;
    private static readonly _sortBy: ObserveAllSortByWithoutQuery = new ObserveAllSortByWithoutQuery();

    constructor() {
        super(Book, true);
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

    static use(sorting?: Sorting): [QueryResultWithState<Book[]>, SetSorting] {
        return useObservableQuery<Book[], ObserveAll>(ObserveAll, undefined, sorting);
    }

    static useWithPaging(pageSize: number, sorting?: Sorting): [QueryResultWithState<Book[]>, SetSorting, SetPage, SetPageSize] {
        return useObservableQueryWithPaging<Book[], ObserveAll>(ObserveAll, new Paging(0, pageSize), undefined, sorting);
    }
}
