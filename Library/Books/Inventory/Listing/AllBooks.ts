/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

/* eslint-disable sort-imports */
// eslint-disable-next-line header/header
import { QueryFor, QueryResultWithState, Sorting, SortingActions, SortingActionsForQuery, Paging } from '@cratis/applications/queries';
import { useQuery, useQueryWithPaging, PerformQuery, SetSorting, SetPage, SetPageSize } from '@cratis/applications.react/queries';
import { Book } from './Book';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/books/inventory');

class AllBooksSortBy {
    private _id: SortingActionsForQuery<Book[]>;
    private _ISBN: SortingActionsForQuery<Book[]>;
    private _title: SortingActionsForQuery<Book[]>;
    private _author: SortingActionsForQuery<Book[]>;
    private _publishedDate: SortingActionsForQuery<Book[]>;

    constructor(readonly query: AllBooks) {
        this._id = new SortingActionsForQuery<Book[]>('id', query);
        this._ISBN = new SortingActionsForQuery<Book[]>('ISBN', query);
        this._title = new SortingActionsForQuery<Book[]>('title', query);
        this._author = new SortingActionsForQuery<Book[]>('author', query);
        this._publishedDate = new SortingActionsForQuery<Book[]>('publishedDate', query);
    }

    get id(): SortingActionsForQuery<Book[]> {
        return this._id;
    }
    get ISBN(): SortingActionsForQuery<Book[]> {
        return this._ISBN;
    }
    get title(): SortingActionsForQuery<Book[]> {
        return this._title;
    }
    get author(): SortingActionsForQuery<Book[]> {
        return this._author;
    }
    get publishedDate(): SortingActionsForQuery<Book[]> {
        return this._publishedDate;
    }
}

class AllBooksSortByWithoutQuery {
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

export class AllBooks extends QueryFor<Book[]> {
    readonly route: string = '/api/books/inventory';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: Book[] = [];
    private readonly _sortBy: AllBooksSortBy;
    private static readonly _sortBy: AllBooksSortByWithoutQuery = new AllBooksSortByWithoutQuery();

    constructor() {
        super(Book, true);
        this._sortBy = new AllBooksSortBy(this);
    }

    get requiredRequestArguments(): string[] {
        return [
        ];
    }

    get sortBy(): AllBooksSortBy {
        return this._sortBy;
    }

    static get sortBy(): AllBooksSortByWithoutQuery {
        return this._sortBy;
    }

    static use(sorting?: Sorting): [QueryResultWithState<Book[]>, PerformQuery, SetSorting] {
        return useQuery<Book[], AllBooks>(AllBooks, undefined, sorting);
    }

    static useWithPaging(pageSize: number, sorting?: Sorting): [QueryResultWithState<Book[]>, PerformQuery, SetSorting, SetPage, SetPageSize] {
        return useQueryWithPaging<Book[], AllBooks>(AllBooks, new Paging(0, pageSize), undefined, sorting);
    }
}
