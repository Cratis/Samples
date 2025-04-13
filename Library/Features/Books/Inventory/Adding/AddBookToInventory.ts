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

const routeTemplate = Handlebars.compile('/api/books/inventory/add');

export interface IAddBookToInventory {
    ISBN?: string;
    title?: string;
    authorId?: Guid;
    initialStockCount?: number;
    publishedDate?: Date;
}

export class AddBookToInventoryValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        ISBN: new Validator(),
        title: new Validator(),
        authorId: new Validator(),
        initialStockCount: new Validator(),
        publishedDate: new Validator(),
    };
}

export class AddBookToInventory extends Command<IAddBookToInventory> implements IAddBookToInventory {
    readonly route: string = '/api/books/inventory/add';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new AddBookToInventoryValidator();

    private _ISBN!: string;
    private _title!: string;
    private _authorId!: Guid;
    private _initialStockCount!: number;
    private _publishedDate!: Date;

    constructor() {
        super(Object, false);
    }

    get requestArguments(): string[] {
        return [
        ];
    }

    get properties(): string[] {
        return [
            'ISBN',
            'title',
            'authorId',
            'initialStockCount',
            'publishedDate',
        ];
    }

    get ISBN(): string {
        return this._ISBN;
    }

    set ISBN(value: string) {
        this._ISBN = value;
        this.propertyChanged('ISBN');
    }
    get title(): string {
        return this._title;
    }

    set title(value: string) {
        this._title = value;
        this.propertyChanged('title');
    }
    get authorId(): Guid {
        return this._authorId;
    }

    set authorId(value: Guid) {
        this._authorId = value;
        this.propertyChanged('authorId');
    }
    get initialStockCount(): number {
        return this._initialStockCount;
    }

    set initialStockCount(value: number) {
        this._initialStockCount = value;
        this.propertyChanged('initialStockCount');
    }
    get publishedDate(): Date {
        return this._publishedDate;
    }

    set publishedDate(value: Date) {
        this._publishedDate = value;
        this.propertyChanged('publishedDate');
    }

    static use(initialValues?: IAddBookToInventory): [AddBookToInventory, SetCommandValues<IAddBookToInventory>, ClearCommandValues] {
        return useCommand<AddBookToInventory, IAddBookToInventory>(AddBookToInventory, initialValues);
    }
}
