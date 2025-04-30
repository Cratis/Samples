/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

/* eslint-disable sort-imports */
// eslint-disable-next-line header/header
import { field } from '@cratis/fundamentals';
import { Guid } from '@cratis/fundamentals';
import { CartItem } from './CartItem';

export class Cart {

    @field(Guid)
    id!: Guid;

    @field(CartItem, true)
    items!: CartItem[];

    @field(Number)
    totalPrice!: number;
}
