import { withViewModel } from '@cratis/applications.react.mvvm';
import { useDialogContext } from '@cratis/applications.react.mvvm/dialogs';
import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';
import { InputText } from 'primereact/inputtext';
import { useState } from 'react';
import { Register } from './Register';
import { AddAuthorViewModel } from './AddAuthorViewModel';

export class AddAuthorRequest {
}

export class AddAuthorResponse {
}

export const AddAuthor = withViewModel<AddAuthorViewModel>(AddAuthorViewModel, ({ viewModel }) => {
    const [name, setName] = useState<string>('');
    const { resolver } = useDialogContext<AddAuthorRequest, AddAuthorResponse>();
    const [registerAuthor] = Register.use();

    const handleAdd = async () => {
        // await viewModel.register(name);

        registerAuthor.name = name;
        await registerAuthor.execute();

        resolver(new AddAuthorResponse());
    };

    return (
        <Dialog header="Add author" visible={true} style={{ width: '50vw' }} onHide={() => resolver(new AddAuthorResponse())}>
            <div className="card flex flex-column md:flex-row gap-3">
                <div className="p-inputgroup flex-1">
                    <span className="p-inputgroup-addon">
                        <i className="pi pi-user"></i>
                    </span>
                    <InputText placeholder="Name" value={name} onChange={e => setName(e.target.value)} />
                </div>
            </div>

            <div className="card flex flex-wrap justify-content-center gap-3 mt-8">
                <Button label="Add" icon="pi pi-check" onClick={handleAdd} autoFocus />
                <Button label="Cancel" icon="pi pi-times" severity='secondary' onClick={() => resolver(new AddAuthorResponse())} />
            </div>
        </Dialog>
    );
});
