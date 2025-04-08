// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Navigate, Route, Routes } from 'react-router-dom';
import { Inventory } from './Inventory/Inventory';
import { Overdue } from './Overdue/Overdue';
import { Reservations } from './Reservations/Reservations';

export const Books = () => {
    return (
        <Routes>
            <Route path='' element={<Navigate to={'inventory'} />} />
            <Route path='inventory' element={<Inventory />} />
            <Route path='reservations' element={<Reservations />} />
            <Route path='overdue' element={<Overdue />} />
        </Routes>
    );
}
