import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { AllAuthors } from './AllAuthors';

const pageSize = 5;

export const Listing = () => {
    const [allAuthorsResult, , setPage] = AllAuthors.useWithPaging(pageSize);

    return (
        <DataTable
            lazy
            paginator
            value={allAuthorsResult.data}
            rows={pageSize}
            totalRecords={allAuthorsResult.paging.totalItems}
            alwaysShowPaginator={false}
            first={allAuthorsResult.paging.page * pageSize}
            onPage={(e) => setPage(e.page ?? 0)}
            scrollable
            scrollHeight={'flex'}
            emptyMessage="No authors found.">
            <Column field="name" header="Name" />
        </DataTable>
    );
}
