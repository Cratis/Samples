/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

/* eslint-disable sort-imports */
/* eslint-disable @typescript-eslint/no-empty-interface */
// eslint-disable-next-line header/header
import { Command, CommandPropertyValidators, CommandValidator } from '@cratis/applications/commands';
import { useCommand, SetCommandValues, ClearCommandValues } from '@cratis/applications.react/commands';
import { Validator } from '@cratis/applications/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/lenders/register');

export interface IRegisterLender {
    firstName?: string;
    lastName?: string;
    address?: string;
    postalCode?: string;
    city?: string;
    socialSecurityNumber?: string;
}

export class RegisterLenderValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        firstName: new Validator(),
        lastName: new Validator(),
        address: new Validator(),
        postalCode: new Validator(),
        city: new Validator(),
        socialSecurityNumber: new Validator(),
    };
}

export class RegisterLender extends Command<IRegisterLender> implements IRegisterLender {
    readonly route: string = '/api/lenders/register';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new RegisterLenderValidator();

    private _firstName!: string;
    private _lastName!: string;
    private _address!: string;
    private _postalCode!: string;
    private _city!: string;
    private _socialSecurityNumber!: string;

    constructor() {
        super(Object, false);
    }

    get requestArguments(): string[] {
        return [
        ];
    }

    get properties(): string[] {
        return [
            'firstName',
            'lastName',
            'address',
            'postalCode',
            'city',
            'socialSecurityNumber',
        ];
    }

    get firstName(): string {
        return this._firstName;
    }

    set firstName(value: string) {
        this._firstName = value;
        this.propertyChanged('firstName');
    }
    get lastName(): string {
        return this._lastName;
    }

    set lastName(value: string) {
        this._lastName = value;
        this.propertyChanged('lastName');
    }
    get address(): string {
        return this._address;
    }

    set address(value: string) {
        this._address = value;
        this.propertyChanged('address');
    }
    get postalCode(): string {
        return this._postalCode;
    }

    set postalCode(value: string) {
        this._postalCode = value;
        this.propertyChanged('postalCode');
    }
    get city(): string {
        return this._city;
    }

    set city(value: string) {
        this._city = value;
        this.propertyChanged('city');
    }
    get socialSecurityNumber(): string {
        return this._socialSecurityNumber;
    }

    set socialSecurityNumber(value: string) {
        this._socialSecurityNumber = value;
        this.propertyChanged('socialSecurityNumber');
    }

    static use(initialValues?: IRegisterLender): [RegisterLender, SetCommandValues<IRegisterLender>, ClearCommandValues] {
        return useCommand<RegisterLender, IRegisterLender>(RegisterLender, initialValues);
    }
}
