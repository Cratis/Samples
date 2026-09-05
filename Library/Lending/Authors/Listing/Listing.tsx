import { Column, DataTableForObservableQuery } from '@cratis/components/DataTables';
import { ObserveAll } from './ObserveAll';

export const Listing = () => (
    <DataTableForObservableQuery query={ObserveAll} emptyMessage="No authors found.">
        <Column field="name" header="Name" />
    </DataTableForObservableQuery>
);
