/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

/* eslint-disable sort-imports */
// eslint-disable-next-line header/header
import { field } from '@cratis/fundamentals';
import { Guid } from '@cratis/fundamentals';

export class Lender {

    @field(Guid)
    id!: Guid;

    @field(String)
    firstName!: string;

    @field(String)
    lastName!: string;

    @field(String)
    address!: string;

    @field(String)
    postalCode!: string;

    @field(String)
    city!: string;

    @field(String)
    socialSecurityNumber!: string;
}
