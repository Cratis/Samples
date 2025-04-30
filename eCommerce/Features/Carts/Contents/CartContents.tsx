import { DataTable } from 'primereact/datatable';
import { GetCart } from './GetCart';
import { Column } from 'primereact/column';
import { Guid } from '@cratis/fundamentals';

export const CartContents = () => {
    const [cart] = GetCart.use({ cartId: Guid.empty })

    return (
        <>
            <DataTable value={cart?.data.items}>
                <Column field="sku" header="SKU" />
                <Column field="price" header="Price" />
            </DataTable>
            <h2>TotalPrice: {cart?.data.totalPrice}</h2>
        </>
    )
};
