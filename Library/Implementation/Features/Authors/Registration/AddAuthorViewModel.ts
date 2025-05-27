import { injectable } from 'tsyringe';
import { Register } from './Register';

@injectable()
export class AddAuthorViewModel {
    constructor(private readonly _register: Register) {
    }

    async register(name: string): Promise<void> {
        this._register.name = name;
        const result = await this._register.execute();
        // Handle the result as needed
    }
}
