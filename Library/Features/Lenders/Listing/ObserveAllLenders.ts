/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

/* eslint-disable sort-imports */
// eslint-disable-next-line header/header
import { ObservableQueryFor, QueryResultWithState, Sorting, SortingActions, SortingActionsForObservableQuery, Paging } from '@cratis/applications/queries';
import { useObservableQuery, useObservableQueryWithPaging, SetSorting, SetPage, SetPageSize } from '@cratis/applications.react/queries';
import { Lender } from './Lender';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/lenders/observe');

class ObserveAllLendersSortBy {
    private _id: SortingActionsForObservableQuery<Lender[]>;
    private _firstName: SortingActionsForObservableQuery<Lender[]>;
    private _lastName: SortingActionsForObservableQuery<Lender[]>;
    private _address: SortingActionsForObservableQuery<Lender[]>;
    private _postalCode: SortingActionsForObservableQuery<Lender[]>;
    private _city: SortingActionsForObservableQuery<Lender[]>;
    private _socialSecurityNumber: SortingActionsForObservableQuery<Lender[]>;

    constructor(readonly query: ObserveAllLenders) {
        this._id = new SortingActionsForObservableQuery<Lender[]>('id', query);
        this._firstName = new SortingActionsForObservableQuery<Lender[]>('firstName', query);
        this._lastName = new SortingActionsForObservableQuery<Lender[]>('lastName', query);
        this._address = new SortingActionsForObservableQuery<Lender[]>('address', query);
        this._postalCode = new SortingActionsForObservableQuery<Lender[]>('postalCode', query);
        this._city = new SortingActionsForObservableQuery<Lender[]>('city', query);
        this._socialSecurityNumber = new SortingActionsForObservableQuery<Lender[]>('socialSecurityNumber', query);
    }

    get id(): SortingActionsForObservableQuery<Lender[]> {
        return this._id;
    }
    get firstName(): SortingActionsForObservableQuery<Lender[]> {
        return this._firstName;
    }
    get lastName(): SortingActionsForObservableQuery<Lender[]> {
        return this._lastName;
    }
    get address(): SortingActionsForObservableQuery<Lender[]> {
        return this._address;
    }
    get postalCode(): SortingActionsForObservableQuery<Lender[]> {
        return this._postalCode;
    }
    get city(): SortingActionsForObservableQuery<Lender[]> {
        return this._city;
    }
    get socialSecurityNumber(): SortingActionsForObservableQuery<Lender[]> {
        return this._socialSecurityNumber;
    }
}

class ObserveAllLendersSortByWithoutQuery {
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

export class ObserveAllLenders extends ObservableQueryFor<Lender[]> {
    readonly route: string = '/api/lenders/observe';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: Lender[] = [];
    private readonly _sortBy: ObserveAllLendersSortBy;
    private static readonly _sortBy: ObserveAllLendersSortByWithoutQuery = new ObserveAllLendersSortByWithoutQuery();

    constructor() {
        super(Lender, true);
        this._sortBy = new ObserveAllLendersSortBy(this);
    }

    get requiredRequestArguments(): string[] {
        return [
        ];
    }

    get sortBy(): ObserveAllLendersSortBy {
        return this._sortBy;
    }

    static get sortBy(): ObserveAllLendersSortByWithoutQuery {
        return this._sortBy;
    }

    static use(sorting?: Sorting): [QueryResultWithState<Lender[]>, SetSorting] {
        return useObservableQuery<Lender[], ObserveAllLenders>(ObserveAllLenders, undefined, sorting);
    }

    static useWithPaging(pageSize: number, sorting?: Sorting): [QueryResultWithState<Lender[]>, SetSorting, SetPage, SetPageSize] {
        return useObservableQueryWithPaging<Lender[], ObserveAllLenders>(ObserveAllLenders, new Paging(0, pageSize), undefined, sorting);
    }
}
