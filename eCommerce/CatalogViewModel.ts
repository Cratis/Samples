import { injectable } from 'tsyringe';
import { AddItemToCart } from './Features/Carts/AddItem/AddItemToCart';
import { Guid } from '@cratis/fundamentals';

@injectable()
export class CatalogViewModel {
    constructor(readonly command: AddItemToCart) {

    }

    async addItemToCart(sku: string) {
        this.command.sku = sku;
        this.command.cartId = Guid.empty;
        await this.command.execute();
    }
}
