// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useNavigate } from 'react-router-dom';
import { Button } from 'primereact/button';
import { GetMyProfile } from '../Listing';

export const MyProfile = () => {
    const navigate = useNavigate();
    const [profileResult] = GetMyProfile.use();
    const profile = profileResult.data;

    return (
        <div style={{ minHeight: '100vh', backgroundColor: 'var(--surface-ground)' }}>
            <div
                className='flex flex-row items-center justify-between px-6 py-4'
                style={{ backgroundColor: 'var(--surface-card)', borderBottom: '1px solid var(--surface-border)' }}
            >
                <div className='flex flex-row items-center gap-3'>
                    <Button
                        icon='pi pi-arrow-left'
                        text
                        onClick={() => navigate('/')}
                        aria-label='Back to My Library'
                    />
                    <h1 className='m-0 text-2xl font-semibold' style={{ color: 'var(--text-color)' }}>
                        My Profile
                    </h1>
                </div>
            </div>

            <div className='p-6'>
                <div
                    className='mx-auto max-w-lg rounded-lg p-8'
                    style={{ backgroundColor: 'var(--surface-card)', border: '1px solid var(--surface-border)' }}
                >
                    {profileResult.isPerforming && (
                        <div className='flex flex-col items-center gap-3 py-8'>
                            <i className='pi pi-spin pi-spinner text-3xl' style={{ color: 'var(--primary-color)' }} />
                            <span style={{ color: 'var(--text-color-secondary)' }}>Loading profile...</span>
                        </div>
                    )}

                    {!profileResult.isPerforming && profile && (
                        <div className='flex flex-col gap-6'>
                            <div className='flex flex-row items-center gap-5'>
                                <div
                                    className='flex h-20 w-20 shrink-0 items-center justify-center rounded-full'
                                    style={{ backgroundColor: 'var(--highlight-bg)' }}
                                >
                                    <i className='pi pi-user text-4xl' style={{ color: 'var(--primary-color)' }} />
                                </div>
                                <div className='flex flex-col gap-1'>
                                    <span className='text-xl font-semibold' style={{ color: 'var(--text-color)' }}>
                                        {profile.name}
                                    </span>
                                    <span className='text-sm' style={{ color: 'var(--text-color-secondary)' }}>
                                        Library Member
                                    </span>
                                </div>
                            </div>

                            <div className='h-px' style={{ backgroundColor: 'var(--surface-border)' }} />

                            <div className='flex flex-col gap-4'>
                                <div className='flex flex-col gap-1'>
                                    <span className='text-xs font-medium uppercase tracking-wide' style={{ color: 'var(--text-color-secondary)' }}>
                                        Full Name
                                    </span>
                                    <span className='text-base' style={{ color: 'var(--text-color)' }}>
                                        {profile.name}
                                    </span>
                                </div>

                                <div className='flex flex-col gap-1'>
                                    <span className='text-xs font-medium uppercase tracking-wide' style={{ color: 'var(--text-color-secondary)' }}>
                                        Email Address
                                    </span>
                                    <span className='text-base' style={{ color: 'var(--text-color)' }}>
                                        {profile.email}
                                    </span>
                                </div>

                                <div className='flex flex-col gap-1'>
                                    <span className='text-xs font-medium uppercase tracking-wide' style={{ color: 'var(--text-color-secondary)' }}>
                                        Member ID
                                    </span>
                                    <span className='font-mono text-sm' style={{ color: 'var(--text-color-secondary)' }}>
                                        {profile.id.toString()}
                                    </span>
                                </div>
                            </div>
                        </div>
                    )}

                    {!profileResult.isPerforming && !profile && (
                        <div className='flex flex-col items-center gap-3 py-8'>
                            <i className='pi pi-exclamation-triangle text-3xl' style={{ color: 'var(--text-color-secondary)' }} />
                            <span style={{ color: 'var(--text-color-secondary)' }}>Profile not found.</span>
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};
