// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useLayoutContext } from '../context/LayoutContext';
import { Button } from 'primereact/button';
import css from './TopBar.module.css';
import { FaBars } from 'react-icons/fa6';
import { useParams } from 'react-router-dom';

export const TopBar = () => {
    const { toggleLeftSidebarOpen } = useLayoutContext();

    return (
        <div className={css.container}>
            <div className={`flex items-center justify-between ${css.leftSide}`}>
                <div className={css.sidebarToggle}>
                    <Button
                        onClick={toggleLeftSidebarOpen}
                        text
                        rounded
                        className='p-2'>
                        <FaBars />
                    </Button>
                </div>
                <div className="flex-1 flex align-center justify-center">
                    <div className="font-extrabold text-2xl m-2">Library</div>
                </div>

            </div>
            <div className="flex-1 flex items-center justify-end px-5 gap-6">
                <div>
                </div>
            </div>
        </div>
    );
};
