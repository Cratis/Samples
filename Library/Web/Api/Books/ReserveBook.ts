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

const routeTemplate = Handlebars.compile('/api/books/reservations');

export interface IReserveBook {
    bookId?: Guid;
}

export class ReserveBookValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        bookId: new Validator(),
    };
}

export class ReserveBook extends Command<IReserveBook> implements IReserveBook {
    readonly route: string = '/api/books/reservations';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new ReserveBookValidator();

    private _bookId!: Guid;

    constructor() {
        super(Object, false);
    }

    get requestArguments(): string[] {
        return [
        ];
    }

    get properties(): string[] {
        return [
            'bookId',
        ];
    }

    get bookId(): Guid {
        return this._bookId;
    }

    set bookId(value: Guid) {
        this._bookId = value;
        this.propertyChanged('bookId');
    }

    static use(initialValues?: IReserveBook): [ReserveBook, SetCommandValues<IReserveBook>, ClearCommandValues] {
        return useCommand<ReserveBook, IReserveBook>(ReserveBook, initialValues);
    }
}
