import { INavigation } from '@cratis/applications.react.mvvm/browser';
import { injectable } from 'tsyringe';

@injectable()
export class HomeViewModel {
    counter: number = 0;

    constructor(readonly navigation: INavigation) {
    }

    increaseCounter() {
        this.counter++;
    }
}
