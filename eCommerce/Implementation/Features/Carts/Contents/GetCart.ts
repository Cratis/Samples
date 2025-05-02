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


export interface GetCartArguments {
    cartId: Guid;
}
export class GetCart extends ObservableQueryFor<Cart, GetCartArguments> {
    readonly route: string = '/api/carts/{cartId}';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: Cart = {} as any;

    constructor() {
        super(Cart, false);
    }

    get requiredRequestArguments(): string[] {
        return [
            'cartId',
        ];
    }


    static use(args?: GetCartArguments): [QueryResultWithState<Cart>] {
        const [result] = useObservableQuery<Cart, GetCart, GetCartArguments>(GetCart, args);
        return [result];
    }
}
