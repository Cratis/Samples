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

const routeTemplate = Handlebars.compile('/api/authors/register');

export interface IRegister {
    name?: string;
}

export class RegisterValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        name: new Validator(),
    };
}

export class Register extends Command<IRegister> implements IRegister {
    readonly route: string = '/api/authors/register';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new RegisterValidator();

    private _name!: string;

    constructor() {
        super(Object, false);
    }

    get requestArguments(): string[] {
        return [
        ];
    }

    get properties(): string[] {
        return [
            'name',
        ];
    }

    get name(): string {
        return this._name;
    }

    set name(value: string) {
        this._name = value;
        this.propertyChanged('name');
    }

    static use(initialValues?: IRegister): [Register, SetCommandValues<IRegister>, ClearCommandValues] {
        return useCommand<Register, IRegister>(Register, initialValues);
    }
}
