/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

/* eslint-disable sort-imports */
// eslint-disable-next-line header/header
import { ObservableQueryFor, QueryResultWithState, Sorting, SortingActions, SortingActionsForObservableQuery, Paging } from '@cratis/applications/queries';
import { useObservableQuery, useObservableQueryWithPaging, SetSorting, SetPage, SetPageSize } from '@cratis/applications.react/queries';
import { Author } from './Author';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/authors/observe');

class ObserveAllSortBy {
    private _id: SortingActionsForObservableQuery<Author[]>;
    private _name: SortingActionsForObservableQuery<Author[]>;

    constructor(readonly query: ObserveAll) {
        this._id = new SortingActionsForObservableQuery<Author[]>('id', query);
        this._name = new SortingActionsForObservableQuery<Author[]>('name', query);
    }

    get id(): SortingActionsForObservableQuery<Author[]> {
        return this._id;
    }
    get name(): SortingActionsForObservableQuery<Author[]> {
        return this._name;
    }
}

class ObserveAllSortByWithoutQuery {
    private _id: SortingActions  = new SortingActions('id');
    private _name: SortingActions  = new SortingActions('name');

    get id(): SortingActions {
        return this._id;
    }
    get name(): SortingActions {
        return this._name;
    }
}

export class ObserveAll extends ObservableQueryFor<Author[]> {
    readonly route: string = '/api/authors/observe';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: Author[] = [];
    private readonly _sortBy: ObserveAllSortBy;
    private static readonly _sortBy: ObserveAllSortByWithoutQuery = new ObserveAllSortByWithoutQuery();

    constructor() {
        super(Author, true);
        this._sortBy = new ObserveAllSortBy(this);
    }

    get requiredRequestArguments(): string[] {
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
