import { DataTable } from 'primereact/datatable';
import { GetCart } from './GetCart';
import { Guid } from '@cratis/fundamentals';
import { Column } from 'primereact/column';

export const CartContents = () => {
    const [cart] = GetCart.use({cartId: Guid.empty});

    return (
        <DataTable value={cart.data.items}>
            <Column field="sku" header="Sku" />
            <Column field="price" header="Price" />
        </DataTable>
    )
};
