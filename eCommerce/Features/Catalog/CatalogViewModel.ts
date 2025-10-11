import { injectable } from 'tsyringe';
import { AddItemToCart } from '../Carts/AddItem/AddItemToCart';
import { Guid } from '@cratis/fundamentals';

@injectable()
export class CatalogViewModel {
    constructor(readonly command: AddItemToCart) {
    }

    async addItemToCart(sku: string, price: number) {
        this.command.cartId = Guid.parse('5675e4e8-50d2-4fed-94aa-372064f67d38');
        this.command.sku = sku;
        this.command.price = price;
        const result = await this.command.execute();
    }
}
