import { get } from 'http';
import { GetAll } from './GetAll';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { ObserveAll } from './ObserveAll';

export const Listing = () => {
    const [observeAllResult, , setPage] = ObserveAll.useWithPaging(50);

    return (
        <>
            <DataTable
                value={observeAllResult.data}>
                <Column field="name" header="Name" />
            </DataTable>
            <button onClick={() => setPage(0)}>First</button>
            <button onClick={() => setPage(1)}>Second</button>
        </>
    );
}
