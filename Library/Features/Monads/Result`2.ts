/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

/* eslint-disable sort-imports */
// eslint-disable-next-line header/header
import { field } from '@cratis/fundamentals';
import { BookReserved } from '../Lending/Reserving/BookReserved';
import { ValidationResult } from '../Applications/Validation/ValidationResult';

export class Result`2 {

    @field(Boolean)
    isSuccess!: boolean;

    @field(Object)
    value!: any;

    @field(Number)
    index!: number;

    @field(Boolean)
    isT0!: boolean;

    @field(Boolean)
    isT1!: boolean;

    @field(BookReserved)
    asT0!: BookReserved;

    @field(ValidationResult)
    asT1!: ValidationResult;
}
