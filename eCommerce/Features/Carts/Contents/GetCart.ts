/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

/* eslint-disable sort-imports */
// eslint-disable-next-line header/header
import { ObservableQueryFor, QueryResultWithState } from '@cratis/applications/queries';
import { useObservableQuery } from '@cratis/applications.react/queries';
import { Guid } from '@cratis/fundamentals';
import { Cart } from './Cart';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/carts/{{cartId}}');


export interface GetCartParameters {
    cartId: Guid;
}
export class GetCart extends ObservableQueryFor<Cart, GetCartParameters> {
    readonly route: string = '/api/carts/{cartId}';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: Cart = {} as any;

    constructor() {
        super(Cart, false);
    }

    get requiredRequestParameters(): string[] {
        return [
            'cartId',
        ];
    }


    static use(args?: GetCartParameters): [QueryResultWithState<Cart>] {
        const [result] = useObservableQuery<Cart, GetCart, GetCartParameters>(GetCart, args);
        return [result];
    }
}
