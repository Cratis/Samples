import { injectable } from 'tsyringe';

@injectable()
export class CatalogViewModel {
    constructor() {

    }

    addItemToCart(sku: string) {
        console.log('Add to cart', sku);
    }
}
