/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

/* eslint-disable sort-imports */
// eslint-disable-next-line header/header
import { ObservableQueryFor, QueryResultWithState } from '@cratis/arc/queries';
import { useObservableQuery } from '@cratis/arc.react/queries';
import { Guid } from '@cratis/fundamentals';
import { Cart } from './Cart';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/carts/contents/cart-by-id?cartId={{cartId}}');


export interface CartByIdParameters {
    cartId: Guid;
}
export class CartById extends ObservableQueryFor<Cart, CartByIdParameters> {
    readonly route: string = '/api/carts/contents/cart-by-id?cartId={cartId}';
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


    static use(args?: CartByIdParameters): [QueryResultWithState<Cart>] {
        const [result] = useObservableQuery<Cart, CartById, CartByIdParameters>(CartById, args);
        return [result];
    }
}
