// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useNavigate } from 'react-router-dom';
import { Button, Surface } from '@cratis/components/Common';
import { ProgressSpinner } from '@cratis/components/Display';
import { FaBook, FaInbox, FaUser } from 'react-icons/fa6';
import { GetMyBorrowedBooks } from './BorrowedBooks';

export const MembersHome = () => {
    const navigate = useNavigate();
    const [borrowedResult] = GetMyBorrowedBooks.use();
    const borrowedBooks = borrowedResult.data ?? [];

    return (
        <div style={{ minHeight: '100vh', backgroundColor: 'var(--cratis-surface-ground)' }}>
            <div
                className='flex flex-row items-center justify-between px-6 py-4'
                style={{ backgroundColor: 'var(--cratis-surface-card)', borderBottom: '1px solid var(--cratis-surface-border)' }}
            >
                <div className='flex flex-row items-center gap-3'>
                    <FaBook aria-hidden='true' className='text-2xl' style={{ color: 'var(--cratis-primary-color)' }} />
                    <h1 className='m-0 text-2xl font-semibold' style={{ color: 'var(--cratis-text-color)' }}>
                        My Library
                    </h1>
                </div>
                <Button
                    label='My Profile'
                    icon={<FaUser aria-hidden='true' />}
                    variant='outline'
                    onClick={() => navigate('/profile')}
                />
            </div>

            <div className='p-6'>
                <div className='mb-4 flex flex-row items-center gap-2'>
                    <FaBook aria-hidden='true' style={{ color: 'var(--cratis-text-color-secondary)' }} />
                    <h2 className='m-0 text-lg font-medium' style={{ color: 'var(--cratis-text-color)' }}>
                        Currently Borrowed
                    </h2>
                    {borrowedBooks.length > 0 && (
                        <span
                            className='rounded-full px-2 py-0.5 text-sm'
                            style={{ backgroundColor: 'var(--cratis-primary-color)', color: 'var(--cratis-primary-color-text)' }}
                        >
                            {borrowedBooks.length}
                        </span>
                    )}
                </div>

                {borrowedResult.isPerforming && (
                    <div role='status' className='flex items-center gap-2' style={{ color: 'var(--cratis-text-color-secondary)' }}>
                        <span aria-hidden='true' style={{ display: 'inline-flex' }}>
                            <ProgressSpinner style={{ width: '1rem', height: '1rem' }} />
                        </span>
                        <span>Loading your books...</span>
                    </div>
                )}

                {!borrowedResult.isPerforming && borrowedBooks.length === 0 && (
                    <Surface className='flex flex-col items-center gap-3 py-16'>
                        <FaInbox aria-hidden='true' className='text-5xl' style={{ color: 'var(--cratis-text-color-secondary)' }} />
                        <p className='m-0 text-base' style={{ color: 'var(--cratis-text-color-secondary)' }}>
                            You have no books borrowed at the moment.
                        </p>
                    </Surface>
                )}

                <div className='grid gap-4' style={{ gridTemplateColumns: 'repeat(auto-fill, minmax(280px, 1fr))' }}>
                    {borrowedBooks.map(book => (
                        <Surface
                            as='article'
                            key={book.id.toString()}
                            className='p-5'
                        >
                            <div className='flex flex-row items-start gap-4'>
                                <div
                                    className='flex h-16 w-12 shrink-0 items-center justify-center rounded'
                                    style={{ backgroundColor: 'var(--cratis-highlight-bg)' }}
                                >
                                    <FaBook aria-hidden='true' className='text-2xl' style={{ color: 'var(--cratis-primary-color)' }} />
                                </div>
                                <div className='flex flex-col gap-1 overflow-hidden'>
                                    <span className='truncate text-base font-semibold' style={{ color: 'var(--cratis-text-color)' }}>
                                        {book.title}
                                    </span>
                                    <span className='text-sm' style={{ color: 'var(--cratis-text-color-secondary)' }}>
                                        ISBN: {book.isbn}
                                    </span>
                                </div>
                            </div>
                        </Surface>
                    ))}
                </div>
            </div>
        </div>
    );
};
