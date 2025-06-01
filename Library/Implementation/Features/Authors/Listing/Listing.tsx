import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { ObserveAll } from './ObserveAll';

const pageSize = 15;

export const Listing = () => {
    const [observeAllResult, , setPage] = ObserveAll.useWithPaging(pageSize);

    return (
        <DataTable
            lazy
            paginator
            value={observeAllResult.data}
            rows={pageSize}
            totalRecords={observeAllResult.paging.totalItems}
            alwaysShowPaginator={false}
            first={observeAllResult.paging.page * pageSize}
            onPage={(e) => setPage(e.page ?? 0)}
            scrollable
            scrollHeight={'flex'}
            emptyMessage="No authors found.">
            <Column field="name" header="Name" />
        </DataTable>
    );
}
