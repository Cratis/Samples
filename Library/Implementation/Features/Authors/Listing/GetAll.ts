/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

/* eslint-disable sort-imports */
// eslint-disable-next-line header/header
import { QueryFor, QueryResultWithState, Sorting, SortingActions, SortingActionsForQuery, Paging } from '@cratis/applications/queries';
import { useQuery, useQueryWithPaging, PerformQuery, SetSorting, SetPage, SetPageSize } from '@cratis/applications.react/queries';
import { Author } from './Author';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/authors');

class GetAllSortBy {
    private _id: SortingActionsForQuery<Author[]>;
    private _name: SortingActionsForQuery<Author[]>;

    constructor(readonly query: GetAll) {
        this._id = new SortingActionsForQuery<Author[]>('id', query);
        this._name = new SortingActionsForQuery<Author[]>('name', query);
    }

    get id(): SortingActionsForQuery<Author[]> {
        return this._id;
    }
    get name(): SortingActionsForQuery<Author[]> {
        return this._name;
    }
}

class GetAllSortByWithoutQuery {
    private _id: SortingActions  = new SortingActions('id');
    private _name: SortingActions  = new SortingActions('name');

    get id(): SortingActions {
        return this._id;
    }
    get name(): SortingActions {
        return this._name;
    }
}

export class GetAll extends QueryFor<Author[]> {
    readonly route: string = '/api/authors';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: Author[] = [];
    private readonly _sortBy: GetAllSortBy;
    private static readonly _sortBy: GetAllSortByWithoutQuery = new GetAllSortByWithoutQuery();

    constructor() {
        super(Author, true);
        this._sortBy = new GetAllSortBy(this);
    }

    get requiredRequestArguments(): string[] {
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
