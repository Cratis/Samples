// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Seeding;
using Members.BorrowedBooks;
using Members.Profiles.Registration;

namespace Members.Seeding;

/// <summary>
/// Seeds the Members event store with a set of well-known library members and their current borrowings
/// so the application starts with meaningful demonstration data.
/// </summary>
/// <remarks>
/// Member IDs are fixed so seed data remains consistent across restarts.
/// The IDs intentionally align with those used by the Lending service seeder (<c>LibrarySeeding</c>),
/// so reservations in the Library event store reference the same logical people registered here.
/// Borrowing record IDs use the same deterministic member and book keys as incoming Lending events.
/// </remarks>
public sealed class MembersSeeding : ICanSeedEvents
{
    static readonly Guid _aliceId = new(0xb2000000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
    static readonly Guid _bobId = new(0xb2000000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2);
    static readonly Guid _carolId = new(0xb2000000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);

    static readonly BorrowedBookId _hobbitBorrowingId = BorrowedBookId.For(new MemberId(_aliceId), "978-0547928227");
    static readonly BorrowedBookId _foundationBorrowingId = BorrowedBookId.For(new MemberId(_aliceId), "978-0553293357");
    static readonly BorrowedBookId _duneBorrowingId = BorrowedBookId.For(new MemberId(_bobId), "978-0441013593");

    /// <inheritdoc/>
    public void Seed(IEventSeedingBuilder builder)
    {
        SeedMembers(builder);
        SeedBorrowings(builder);
    }

    static void SeedMembers(IEventSeedingBuilder builder) =>
        builder
            .For(_aliceId, [new MemberRegistered("Alice Johnson", "alice@library.example.com")])
            .For(_bobId, [new MemberRegistered("Bob Smith", "bob@library.example.com")])
            .For(_carolId, [new MemberRegistered("Carol White", "carol@library.example.com")]);

    static void SeedBorrowings(IEventSeedingBuilder builder)
    {
        var borrowedDate = DateTimeOffset.UtcNow.AddDays(-5);
        var dueDate = borrowedDate.AddDays(21);

        builder
            .For(_hobbitBorrowingId, [new BookLendingRecorded("978-0547928227", "The Hobbit", new MemberId(_aliceId), borrowedDate, dueDate)])
            .For(_foundationBorrowingId, [new BookLendingRecorded("978-0553293357", "Foundation", new MemberId(_aliceId), borrowedDate, dueDate)])
            .For(_duneBorrowingId, [new BookLendingRecorded("978-0441013593", "Dune", new MemberId(_bobId), borrowedDate, dueDate)]);
    }
}
