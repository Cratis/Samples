// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { HTMLAttributes, ReactNode } from 'react';

export interface PageProps extends HTMLAttributes<HTMLDivElement> {
    title: string;
    children?: ReactNode;
}

export const Page = ({ title, children, ...rest }: PageProps) => {
    return (
        <div className='flex flex-col h-full' {...rest}>
            <h1 className='text-3xl mt-3 mb-4'>{title}</h1>
            <main className={`panel overflow-hidden h-full flex flex-col flex-1`}>
                {children}
            </main>
        </div>
    );
};
