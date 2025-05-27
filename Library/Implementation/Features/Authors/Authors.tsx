import { useDialogRequest } from '@cratis/applications.react.mvvm/dialogs';
import { Menubar } from 'primereact/menubar'
import { MenuItem } from 'primereact/menuitem';
import * as mdIcons from 'react-icons/md';
import { AddAuthor, AddAuthorRequest, AddAuthorResponse } from './Registration/AddAuthor';
import { withViewModel } from '@cratis/applications.react.mvvm';
import { AuthorsViewModel } from './AuthorsViewModel';
import { Listing } from './Listing/Listing';
import { useDialog } from './useDialog';

export const Authors = withViewModel(AuthorsViewModel, ({ viewModel }) => {
    const [AddAuthorDialogWrapper] = useDialogRequest<AddAuthorRequest, AddAuthorResponse>(AddAuthorRequest);

    const [AddAuthorDialog, showAddAuthorDialog] = useDialog(AddAuthor);

    const menuItems: MenuItem[] = [
        {
            label: 'Add Author',
            icon: mdIcons.MdPersonAdd,
            command: async () => {
                const result = await showAddAuthorDialog();
                // viewModel.addAuthor();
            }
        }
    ];

    return (
        <div>
            <Menubar model={menuItems} />
            <Listing/>

            <AddAuthorDialog/>

            {/* <AddAuthorDialogWrapper>
                <AddAuthor />
            </AddAuthorDialogWrapper> */}
        </div>
    );
});



