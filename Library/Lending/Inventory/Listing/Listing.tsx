import { Column, DataTableForObservableQuery } from '@cratis/components/DataTables';
import { ObserveAll } from './ObserveAll';

export const Listing = () => (
    <DataTableForObservableQuery query={ObserveAll} emptyMessage="No books found.">
        <Column field="title" header="Name" />
    </DataTableForObservableQuery>
);
