/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

/* eslint-disable sort-imports */
/* eslint-disable @typescript-eslint/no-empty-interface */
// eslint-disable-next-line header/header
import { Command, CommandPropertyValidators, CommandValidator } from '@cratis/applications/commands';
import { useCommand, SetCommandValues, ClearCommandValues } from '@cratis/applications.react/commands';
import { Validator } from '@cratis/applications/validation';
import { Guid } from '@cratis/fundamentals';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/carts/{{cartId}}/items');

export interface IAddItemToCart {
    cartId?: Guid;
    sku?: string;
}

export class AddItemToCartValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        cartId: new Validator(),
        sku: new Validator(),
    };
}

export class AddItemToCart extends Command<IAddItemToCart> implements IAddItemToCart {
    readonly route: string = '/api/carts/{cartId}/items';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new AddItemToCartValidator();

    private _cartId!: Guid;
    private _sku!: string;

    constructor() {
        super(Object, false);
    }

    get requestArguments(): string[] {
        return [
            'cartId',
        ];
    }

    get properties(): string[] {
        return [
            'cartId',
            'sku',
        ];
    }

    get cartId(): Guid {
        return this._cartId;
    }

    set cartId(value: Guid) {
        this._cartId = value;
        this.propertyChanged('cartId');
    }
    get sku(): string {
        return this._sku;
    }

    set sku(value: string) {
        this._sku = value;
        this.propertyChanged('sku');
    }

    static use(initialValues?: IAddItemToCart): [AddItemToCart, SetCommandValues<IAddItemToCart>, ClearCommandValues] {
        return useCommand<AddItemToCart, IAddItemToCart>(AddItemToCart, initialValues);
    }
}
