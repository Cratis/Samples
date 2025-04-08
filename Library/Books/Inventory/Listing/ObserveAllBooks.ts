/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

/* eslint-disable sort-imports */
// eslint-disable-next-line header/header
import { ObservableQueryFor, QueryResultWithState, Sorting, SortingActions, SortingActionsForObservableQuery, Paging } from '@cratis/applications/queries';
import { useObservableQuery, useObservableQueryWithPaging, SetSorting, SetPage, SetPageSize } from '@cratis/applications.react/queries';
import { Book } from './Book';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/books/inventory/observe');

class ObserveAllBooksSortBy {
    private _id: SortingActionsForObservableQuery<Book[]>;
    private _ISBN: SortingActionsForObservableQuery<Book[]>;
    private _title: SortingActionsForObservableQuery<Book[]>;
    private _author: SortingActionsForObservableQuery<Book[]>;
    private _publishedDate: SortingActionsForObservableQuery<Book[]>;

    constructor(readonly query: ObserveAllBooks) {
        this._id = new SortingActionsForObservableQuery<Book[]>('id', query);
        this._ISBN = new SortingActionsForObservableQuery<Book[]>('ISBN', query);
        this._title = new SortingActionsForObservableQuery<Book[]>('title', query);
        this._author = new SortingActionsForObservableQuery<Book[]>('author', query);
        this._publishedDate = new SortingActionsForObservableQuery<Book[]>('publishedDate', query);
    }

    get id(): SortingActionsForObservableQuery<Book[]> {
        return this._id;
    }
    get ISBN(): SortingActionsForObservableQuery<Book[]> {
        return this._ISBN;
    }
    get title(): SortingActionsForObservableQuery<Book[]> {
        return this._title;
    }
    get author(): SortingActionsForObservableQuery<Book[]> {
        return this._author;
    }
    get publishedDate(): SortingActionsForObservableQuery<Book[]> {
        return this._publishedDate;
    }
}

class ObserveAllBooksSortByWithoutQuery {
    private _id: SortingActions  = new SortingActions('id');
    private _ISBN: SortingActions  = new SortingActions('ISBN');
    private _title: SortingActions  = new SortingActions('title');
    private _author: SortingActions  = new SortingActions('author');
    private _publishedDate: SortingActions  = new SortingActions('publishedDate');

    get id(): SortingActions {
        return this._id;
    }
    get ISBN(): SortingActions {
        return this._ISBN;
    }
    get title(): SortingActions {
        return this._title;
    }
    get author(): SortingActions {
        return this._author;
    }
    get publishedDate(): SortingActions {
        return this._publishedDate;
    }
}

export class ObserveAllBooks extends ObservableQueryFor<Book[]> {
    readonly route: string = '/api/books/inventory/observe';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: Book[] = [];
    private readonly _sortBy: ObserveAllBooksSortBy;
    private static readonly _sortBy: ObserveAllBooksSortByWithoutQuery = new ObserveAllBooksSortByWithoutQuery();

    constructor() {
        super(Book, true);
        this._sortBy = new ObserveAllBooksSortBy(this);
    }

    get requiredRequestArguments(): string[] {
        return [
        ];
    }

    get sortBy(): ObserveAllBooksSortBy {
        return this._sortBy;
    }

    static get sortBy(): ObserveAllBooksSortByWithoutQuery {
        return this._sortBy;
    }

    static use(sorting?: Sorting): [QueryResultWithState<Book[]>, SetSorting] {
        return useObservableQuery<Book[], ObserveAllBooks>(ObserveAllBooks, undefined, sorting);
    }

    static useWithPaging(pageSize: number, sorting?: Sorting): [QueryResultWithState<Book[]>, SetSorting, SetPage, SetPageSize] {
        return useObservableQueryWithPaging<Book[], ObserveAllBooks>(ObserveAllBooks, new Paging(0, pageSize), undefined, sorting);
    }
}
