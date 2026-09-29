// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useNavigate } from 'react-router-dom';
import { IconButton, Surface } from '@cratis/components/Common';
import { ProgressSpinner } from '@cratis/components/Display';
import { FaArrowLeft, FaTriangleExclamation, FaUser } from 'react-icons/fa6';
import { GetMyProfile } from '../Listing';

export const MyProfile = () => {
    const navigate = useNavigate();
    const [profileResult] = GetMyProfile.use();
    const profile = profileResult.data;

    return (
        <div style={{ minHeight: '100vh', backgroundColor: 'var(--cratis-surface-ground)' }}>
            <div
                className='flex flex-row items-center justify-between px-6 py-4'
                style={{ backgroundColor: 'var(--cratis-surface-card)', borderBottom: '1px solid var(--cratis-surface-border)' }}
            >
                <div className='flex flex-row items-center gap-3'>
                    <IconButton
                        icon={<FaArrowLeft aria-hidden='true' />}
                        variant='ghost'
                        onClick={() => navigate('/')}
                        aria-label='Back to My Library'
                    />
                    <h1 className='m-0 text-2xl font-semibold' style={{ color: 'var(--cratis-text-color)' }}>
                        My Profile
                    </h1>
                </div>
            </div>

            <div className='p-6'>
                <Surface as='section' aria-label='Profile' className='mx-auto max-w-lg p-8'>
                    {/* Always mounted so assistive technology announces the text when it appears. */}
                    <div role='status'>
                        {profileResult.isPerforming && (
                            <div className='flex flex-col items-center gap-3 py-8'>
                                <span aria-hidden='true' style={{ display: 'inline-flex' }}>
                                    <ProgressSpinner style={{ width: '1.875rem', height: '1.875rem' }} />
                                </span>
                                <span style={{ color: 'var(--cratis-text-color-secondary)' }}>Loading profile...</span>
                            </div>
                        )}
                    </div>

                    {!profileResult.isPerforming && profile && (
                        <div className='flex flex-col gap-6'>
                            <div className='flex flex-row items-center gap-5'>
                                <div
                                    className='flex h-20 w-20 shrink-0 items-center justify-center rounded-full'
                                    style={{ backgroundColor: 'var(--cratis-highlight-bg)' }}
                                >
                                    <FaUser aria-hidden='true' className='text-4xl' style={{ color: 'var(--cratis-primary-color)' }} />
                                </div>
                                <div className='flex flex-col gap-1'>
                                    <span className='text-xl font-semibold' style={{ color: 'var(--cratis-text-color)' }}>
                                        {profile.name}
                                    </span>
                                    <span className='text-sm' style={{ color: 'var(--cratis-text-color-secondary)' }}>
                                        Library Member
                                    </span>
                                </div>
                            </div>

                            <div className='h-px' style={{ backgroundColor: 'var(--cratis-surface-border)' }} />

                            <div className='flex flex-col gap-4'>
                                <div className='flex flex-col gap-1'>
                                    <span className='text-xs font-medium uppercase tracking-wide' style={{ color: 'var(--cratis-text-color-secondary)' }}>
                                        Full Name
                                    </span>
                                    <span className='text-base' style={{ color: 'var(--cratis-text-color)' }}>
                                        {profile.name}
                                    </span>
                                </div>

                                <div className='flex flex-col gap-1'>
                                    <span className='text-xs font-medium uppercase tracking-wide' style={{ color: 'var(--cratis-text-color-secondary)' }}>
                                        Email Address
                                    </span>
                                    <span className='text-base' style={{ color: 'var(--cratis-text-color)' }}>
                                        {profile.email}
                                    </span>
                                </div>

                                <div className='flex flex-col gap-1'>
                                    <span className='text-xs font-medium uppercase tracking-wide' style={{ color: 'var(--cratis-text-color-secondary)' }}>
                                        Member ID
                                    </span>
                                    <span className='font-mono text-sm' style={{ color: 'var(--cratis-text-color-secondary)' }}>
                                        {profile.id.toString()}
                                    </span>
                                </div>
                            </div>
                        </div>
                    )}

                    {!profileResult.isPerforming && !profile && (
                        <div className='flex flex-col items-center gap-3 py-8'>
                            <FaTriangleExclamation aria-hidden='true' className='text-3xl' style={{ color: 'var(--cratis-text-color-secondary)' }} />
                            <span style={{ color: 'var(--cratis-text-color-secondary)' }}>Profile not found.</span>
                        </div>
                    )}
                </Surface>
            </div>
        </div>
    );
};
