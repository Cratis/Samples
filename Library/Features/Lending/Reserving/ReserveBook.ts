/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

/* eslint-disable sort-imports */
/* eslint-disable @typescript-eslint/no-empty-interface */
// eslint-disable-next-line header/header
import { Command, CommandPropertyValidators, CommandValidator } from '@cratis/applications/commands';
import { useCommand, SetCommandValues, ClearCommandValues } from '@cratis/applications.react/commands';
import { Validator } from '@cratis/applications/validation';
import { PropertyDescriptor } from '@cratis/applications/reflection';
import { Guid } from '@cratis/fundamentals';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/lending/reserving');

export interface IReserveBook {
    isbn?: string;
    memberId?: Guid;
}

export class ReserveBookValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        isbn: new Validator(),
        memberId: new Validator(),
    };
}

export class ReserveBook extends Command<IReserveBook> implements IReserveBook {
    readonly route: string = '/api/lending/reserving';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new ReserveBookValidator();
    readonly propertyDescriptors: PropertyDescriptor[] = [
        new PropertyDescriptor('isbn', String),
        new PropertyDescriptor('memberId', Guid),
    ];

    private _isbn!: string;
    private _memberId!: Guid;

    constructor() {
        super(Object, false);
    }

    get requestParameters(): string[] {
        return [
        ];
    }

    get properties(): string[] {
        return [
            'isbn',
            'memberId',
        ];
    }

    get isbn(): string {
        return this._isbn;
    }

    set isbn(value: string) {
        this._isbn = value;
        this.propertyChanged('isbn');
    }
    get memberId(): Guid {
        return this._memberId;
    }

    set memberId(value: Guid) {
        this._memberId = value;
        this.propertyChanged('memberId');
    }

    static use(initialValues?: IReserveBook): [ReserveBook, SetCommandValues<IReserveBook>, ClearCommandValues] {
        return useCommand<ReserveBook, IReserveBook>(ReserveBook, initialValues);
    }
}
