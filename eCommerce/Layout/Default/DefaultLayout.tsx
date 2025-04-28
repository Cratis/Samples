// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { LayoutContext } from './context/LayoutContext';
import { generatePath, Outlet, useParams } from 'react-router-dom';
import { IMenuItemGroup } from './Sidebar/MenuItem/MenuItem';
import { MenuProvider } from './context/MenuContext';
import { SidebarMenu } from './Sidebar/SidebarMenu';
import css from './DefaultLayout.module.css';
import { TopBar } from './TopBar/TopBar';
import { Footer } from './Footer';
import { ErrorBoundary } from 'Components/Common/ErrorBoundary';
import { useContext, useState } from 'react';

interface IDefaultLayoutProps {
    menu?: IMenuItemGroup[];
    basePath?: string;
}

export const DefaultLayout = (props: IDefaultLayoutProps) => {
    const sidebarBasePath = generatePath(props.basePath ?? '');
    const layoutContext = useContext(LayoutContext);

    return (
        <ErrorBoundary>
            <div
                className={`${!layoutContext.layoutConfig.leftSidebarOpen
                    ? css.sidebarClosed
                    : ''
                    }`}>
                <header className={css.appHeader}>
                    <TopBar />
                </header>

                <aside className={css.appLeftSidebar}>
                    <div className={css.sidebarContainer}>
                        {props.menu && (
                            <MenuProvider>
                                <SidebarMenu
                                    items={props.menu}
                                    basePath={sidebarBasePath}
                                />
                            </MenuProvider>
                        )}
                    </div>
                </aside>

                <main className={css.appOutlet}>
                    <ErrorBoundary>
                        <div className='px-6 py-4 flex flex-col h-full'>
                            <Outlet />
                        </div>
                    </ErrorBoundary>
                </main>
                <footer className={css.appFooter}>
                    <Footer />
                </footer>
            </div>
        </ErrorBoundary>
    );
};
