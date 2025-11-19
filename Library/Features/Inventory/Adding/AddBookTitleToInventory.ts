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
import { BookAddedToInventory } from './BookAddedToInventory';
import { Guid } from '@cratis/fundamentals';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/inventory/adding');

export interface IAddBookTitleToInventory {
    title?: string;
    ISBN?: string;
    authorId?: Guid;
    count?: number;
}

export class AddBookTitleToInventoryValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        title: new Validator(),
        ISBN: new Validator(),
        authorId: new Validator(),
        count: new Validator(),
    };
}

export class AddBookTitleToInventory extends Command<IAddBookTitleToInventory, BookAddedToInventory> implements IAddBookTitleToInventory {
    readonly route: string = '/api/inventory/adding';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new AddBookTitleToInventoryValidator();
    readonly propertyDescriptors: PropertyDescriptor[] = [
        new PropertyDescriptor('title', String),
        new PropertyDescriptor('ISBN', String),
        new PropertyDescriptor('authorId', Guid),
        new PropertyDescriptor('count', Number),
    ];

    private _title!: string;
    private _ISBN!: string;
    private _authorId!: Guid;
    private _count!: number;

    constructor() {
        super(BookAddedToInventory, false);
    }

    get requestParameters(): string[] {
        return [
        ];
    }

    get properties(): string[] {
        return [
            'title',
            'ISBN',
            'authorId',
            'count',
        ];
    }

    get title(): string {
        return this._title;
    }

    set title(value: string) {
        this._title = value;
        this.propertyChanged('title');
    }
    get ISBN(): string {
        return this._ISBN;
    }

    set ISBN(value: string) {
        this._ISBN = value;
        this.propertyChanged('ISBN');
    }
    get authorId(): Guid {
        return this._authorId;
    }

    set authorId(value: Guid) {
        this._authorId = value;
        this.propertyChanged('authorId');
    }
    get count(): number {
        return this._count;
    }

    set count(value: number) {
        this._count = value;
        this.propertyChanged('count');
    }

    static use(initialValues?: IAddBookTitleToInventory): [AddBookTitleToInventory, SetCommandValues<IAddBookTitleToInventory>, ClearCommandValues] {
        return useCommand<AddBookTitleToInventory, IAddBookTitleToInventory>(AddBookTitleToInventory, initialValues);
    }
}
