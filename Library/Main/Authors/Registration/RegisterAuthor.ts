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

const routeTemplate = Handlebars.compile('/api/authors/register');

export interface IRegisterAuthor {
    authorId?: Guid;
    name?: string;
}

export class RegisterAuthorValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        authorId: new Validator(),
        name: new Validator(),
    };
}

export class RegisterAuthor extends Command<IRegisterAuthor> implements IRegisterAuthor {
    readonly route: string = '/api/authors/register';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new RegisterAuthorValidator();

    private _authorId!: Guid;
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
            'authorId',
            'name',
        ];
    }

    get authorId(): Guid {
        return this._authorId;
    }

    set authorId(value: Guid) {
        this._authorId = value;
        this.propertyChanged('authorId');
    }
    get name(): string {
        return this._name;
    }

    set name(value: string) {
        this._name = value;
        this.propertyChanged('name');
    }

    static use(initialValues?: IRegisterAuthor): [RegisterAuthor, SetCommandValues<IRegisterAuthor>, ClearCommandValues] {
        return useCommand<RegisterAuthor, IRegisterAuthor>(RegisterAuthor, initialValues);
    }
}
