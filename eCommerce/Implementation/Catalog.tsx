// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { withViewModel } from '@cratis/applications.react.mvvm';
import { Column } from 'primereact/column';
import { DataTable } from 'primereact/datatable';
import { CatalogViewModel } from './CatalogViewModel';

export const Catalog = withViewModel(CatalogViewModel, ({ viewModel }) => {
    const products = [
        { sku: '123', description: 'Product 1', price: 42 },
        { sku: '456', description: 'Product 2', price: 34 },
        { sku: '789', description: 'Product 3', price: 83 },
    ];

    const addToCartTemplate = (item) => {
        return (
            <button
                className="p-button p-component p-button-text-only"
                onClick={() => viewModel.addItemToCart(item.sku, item.price)}>
                <span className="p-button-text">Add to Cart</span>
            </button>
        )
    };

    return (
        <DataTable value={products}>
            <Column field="sku" header="Sku" />
            <Column field="description" header="Description" />
            <Column field="price" header="Price" />
            <Column body={addToCartTemplate}/>
        </DataTable>
    );
});
