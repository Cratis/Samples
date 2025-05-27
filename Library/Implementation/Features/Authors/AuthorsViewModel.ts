import { IDialogs } from '@cratis/applications.react.mvvm/dialogs';
import { injectable } from 'tsyringe';
import { AddAuthorRequest, AddAuthorResponse } from './Registration/AddAuthor';

@injectable()
export class AuthorsViewModel {
    constructor(private readonly dialogs: IDialogs) {
    }

    async addAuthor() {
        await this.dialogs.show<AddAuthorRequest, AddAuthorResponse>(new AddAuthorRequest());
    }
}
