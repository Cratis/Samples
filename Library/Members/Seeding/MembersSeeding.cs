// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Seeding;
using Members.Profiles.Registration;

namespace Members.Seeding;

/// <summary>
/// Seeds the Members event store with a set of well-known library members so the application
/// starts with meaningful demonstration data.
/// </summary>
/// <remarks>
/// Member IDs are fixed so seed data remains consistent across restarts.
/// The IDs intentionally align with those used by the Lending service seeder (<c>LibrarySeeding</c>),
/// so reservations in the Library event store reference the same logical people registered here.
/// </remarks>
public sealed class MembersSeeding : ICanSeedEvents
{
    static readonly Guid _aliceId = new(0xb2000000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
    static readonly Guid _bobId = new(0xb2000000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2);
    static readonly Guid _carolId = new(0xb2000000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);

    /// <inheritdoc/>
    public void Seed(IEventSeedingBuilder builder) =>
        builder
            .For<MemberRegistered>(_aliceId, [new MemberRegistered("Alice Johnson", "alice@library.example.com")])
            .For<MemberRegistered>(_bobId, [new MemberRegistered("Bob Smith", "bob@library.example.com")])
            .For<MemberRegistered>(_carolId, [new MemberRegistered("Carol White", "carol@library.example.com")]);
}
