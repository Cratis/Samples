// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useNavigate } from 'react-router-dom';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { GetMyBorrowedBooks } from './BorrowedBooks';

export const MembersHome = () => {
    const navigate = useNavigate();
    const [borrowedResult] = GetMyBorrowedBooks.use();
    const borrowedBooks = borrowedResult.data ?? [];

    return (
        <div style={{ minHeight: '100vh', backgroundColor: 'var(--surface-ground)' }}>
            <div
                className='flex flex-row items-center justify-between px-6 py-4'
                style={{ backgroundColor: 'var(--surface-card)', borderBottom: '1px solid var(--surface-border)' }}
            >
                <div className='flex flex-row items-center gap-3'>
                    <i className='pi pi-book text-2xl' style={{ color: 'var(--primary-color)' }} />
                    <h1 className='m-0 text-2xl font-semibold' style={{ color: 'var(--text-color)' }}>
                        My Library
                    </h1>
                </div>
                <Button
                    label='My Profile'
                    icon='pi pi-user'
                    outlined
                    onClick={() => navigate('/profile')}
                />
            </div>

            <div className='p-6'>
                <div className='mb-4 flex flex-row items-center gap-2'>
                    <i className='pi pi-book' style={{ color: 'var(--text-color-secondary)' }} />
                    <h2 className='m-0 text-lg font-medium' style={{ color: 'var(--text-color)' }}>
                        Currently Borrowed
                    </h2>
                    {borrowedBooks.length > 0 && (
                        <span
                            className='rounded-full px-2 py-0.5 text-sm'
                            style={{ backgroundColor: 'var(--primary-color)', color: 'var(--primary-color-text)' }}
                        >
                            {borrowedBooks.length}
                        </span>
                    )}
                </div>

                {borrowedResult.isPerforming && (
                    <div className='flex items-center gap-2' style={{ color: 'var(--text-color-secondary)' }}>
                        <i className='pi pi-spin pi-spinner' />
                        <span>Loading your books...</span>
                    </div>
                )}

                {!borrowedResult.isPerforming && borrowedBooks.length === 0 && (
                    <div
                        className='flex flex-col items-center gap-3 rounded-lg py-16'
                        style={{ backgroundColor: 'var(--surface-card)', border: '1px solid var(--surface-border)' }}
                    >
                        <i className='pi pi-inbox text-5xl' style={{ color: 'var(--text-color-secondary)' }} />
                        <p className='m-0 text-base' style={{ color: 'var(--text-color-secondary)' }}>
                            You have no books borrowed at the moment.
                        </p>
                    </div>
                )}

                <div className='grid gap-4' style={{ gridTemplateColumns: 'repeat(auto-fill, minmax(280px, 1fr))' }}>
                    {borrowedBooks.map(book => (
                        <Card
                            key={book.id.toString()}
                            style={{ border: '1px solid var(--surface-border)' }}
                        >
                            <div className='flex flex-row items-start gap-4'>
                                <div
                                    className='flex h-16 w-12 shrink-0 items-center justify-center rounded'
                                    style={{ backgroundColor: 'var(--highlight-bg)' }}
                                >
                                    <i className='pi pi-book text-2xl' style={{ color: 'var(--primary-color)' }} />
                                </div>
                                <div className='flex flex-col gap-1 overflow-hidden'>
                                    <span className='truncate text-base font-semibold' style={{ color: 'var(--text-color)' }}>
                                        {book.title}
                                    </span>
                                    <span className='text-sm' style={{ color: 'var(--text-color-secondary)' }}>
                                        ISBN: {book.isbn}
                                    </span>
                                </div>
                            </div>
                        </Card>
                    ))}
                </div>
            </div>
        </div>
    );
};
