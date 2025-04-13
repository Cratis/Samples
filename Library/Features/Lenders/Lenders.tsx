// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Column } from 'primereact/column';
import { DataPage, MenuItem } from '../../Components/DataPage';
import { ObserveAllLenders } from './Listing';
import * as faIcons from 'react-icons/fa';
import { RegisterLenderView } from './Registration/RegisterLenderView';
import { useState } from 'react';

export const Lenders = () => {
    const [addLenderDialogVisible, setAddLenderDialogVisible] = useState(false);

    return (
        <>
            <DataPage
                title='Lenders'
                query={ObserveAllLenders}
                dataKey='id'
                emptyMessage='No lenders found'>

                <DataPage.MenuItems>
                    <MenuItem id='add' label='Add Lender' icon={faIcons.FaPlus} command={() => setAddLenderDialogVisible(true)} />
                </DataPage.MenuItems>

                <DataPage.Columns>
                    <Column field='firstName' header='First Name' />
                    <Column field='lastName' header='Last Name' />
                </DataPage.Columns>
            </DataPage>
            <RegisterLenderView visible={addLenderDialogVisible} onHide={() => setAddLenderDialogVisible(false)} />
        </>
    );
};
