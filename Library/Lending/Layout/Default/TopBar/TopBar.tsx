// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useLayoutContext } from '../context/LayoutContext';
import { IconButton } from '@cratis/components/Common';
import css from './TopBar.module.css';
import { FaBars } from 'react-icons/fa6';

export const TopBar = () => {
    const { toggleLeftSidebarOpen } = useLayoutContext();

    return (
        <div className={css.container}>
            <div className={css.leftSide}>
                <div className={css.sidebarToggle}>
                    <IconButton
                        icon={<FaBars aria-hidden="true" />}
                        aria-label="Toggle sidebar"
                        variant="ghost"
                        onClick={toggleLeftSidebarOpen} />
                </div>
                <div className={css.title}>Library</div>
            </div>
            <div className="flex-1 flex items-center justify-end px-5 gap-6">
                <div>
                </div>
            </div>
        </div>
    );
};
