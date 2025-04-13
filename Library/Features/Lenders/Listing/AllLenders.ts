/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

/* eslint-disable sort-imports */
// eslint-disable-next-line header/header
import { QueryFor, QueryResultWithState, Sorting, SortingActions, SortingActionsForQuery, Paging } from '@cratis/applications/queries';
import { useQuery, useQueryWithPaging, PerformQuery, SetSorting, SetPage, SetPageSize } from '@cratis/applications.react/queries';
import { Lender } from './Lender';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/lenders');

class AllLendersSortBy {
    private _id: SortingActionsForQuery<Lender[]>;
    private _firstName: SortingActionsForQuery<Lender[]>;
    private _lastName: SortingActionsForQuery<Lender[]>;
    private _address: SortingActionsForQuery<Lender[]>;
    private _postalCode: SortingActionsForQuery<Lender[]>;
    private _city: SortingActionsForQuery<Lender[]>;
    private _socialSecurityNumber: SortingActionsForQuery<Lender[]>;

    constructor(readonly query: AllLenders) {
        this._id = new SortingActionsForQuery<Lender[]>('id', query);
        this._firstName = new SortingActionsForQuery<Lender[]>('firstName', query);
        this._lastName = new SortingActionsForQuery<Lender[]>('lastName', query);
        this._address = new SortingActionsForQuery<Lender[]>('address', query);
        this._postalCode = new SortingActionsForQuery<Lender[]>('postalCode', query);
        this._city = new SortingActionsForQuery<Lender[]>('city', query);
        this._socialSecurityNumber = new SortingActionsForQuery<Lender[]>('socialSecurityNumber', query);
    }

    get id(): SortingActionsForQuery<Lender[]> {
        return this._id;
    }
    get firstName(): SortingActionsForQuery<Lender[]> {
        return this._firstName;
    }
    get lastName(): SortingActionsForQuery<Lender[]> {
        return this._lastName;
    }
    get address(): SortingActionsForQuery<Lender[]> {
        return this._address;
    }
    get postalCode(): SortingActionsForQuery<Lender[]> {
        return this._postalCode;
    }
    get city(): SortingActionsForQuery<Lender[]> {
        return this._city;
    }
    get socialSecurityNumber(): SortingActionsForQuery<Lender[]> {
        return this._socialSecurityNumber;
    }
}

class AllLendersSortByWithoutQuery {
    private _id: SortingActions  = new SortingActions('id');
    private _firstName: SortingActions  = new SortingActions('firstName');
    private _lastName: SortingActions  = new SortingActions('lastName');
    private _address: SortingActions  = new SortingActions('address');
    private _postalCode: SortingActions  = new SortingActions('postalCode');
    private _city: SortingActions  = new SortingActions('city');
    private _socialSecurityNumber: SortingActions  = new SortingActions('socialSecurityNumber');

    get id(): SortingActions {
        return this._id;
    }
    get firstName(): SortingActions {
        return this._firstName;
    }
    get lastName(): SortingActions {
        return this._lastName;
    }
    get address(): SortingActions {
        return this._address;
    }
    get postalCode(): SortingActions {
        return this._postalCode;
    }
    get city(): SortingActions {
        return this._city;
    }
    get socialSecurityNumber(): SortingActions {
        return this._socialSecurityNumber;
    }
}

export class AllLenders extends QueryFor<Lender[]> {
    readonly route: string = '/api/lenders';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: Lender[] = [];
    private readonly _sortBy: AllLendersSortBy;
    private static readonly _sortBy: AllLendersSortByWithoutQuery = new AllLendersSortByWithoutQuery();

    constructor() {
        super(Lender, true);
        this._sortBy = new AllLendersSortBy(this);
    }

    get requiredRequestArguments(): string[] {
        return [
        ];
    }

    get sortBy(): AllLendersSortBy {
        return this._sortBy;
    }

    static get sortBy(): AllLendersSortByWithoutQuery {
        return this._sortBy;
    }

    static use(sorting?: Sorting): [QueryResultWithState<Lender[]>, PerformQuery, SetSorting] {
        return useQuery<Lender[], AllLenders>(AllLenders, undefined, sorting);
    }

    static useWithPaging(pageSize: number, sorting?: Sorting): [QueryResultWithState<Lender[]>, PerformQuery, SetSorting, SetPage, SetPageSize] {
        return useQueryWithPaging<Lender[], AllLenders>(AllLenders, new Paging(0, pageSize), undefined, sorting);
    }
}
