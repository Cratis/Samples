// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Seeding;
using Library.Authors.Registration;
using Library.Inventory.Adding;
using Library.Lending.Reserving;

namespace Library.Seeding;

/// <summary>
/// Seeds the Library event store with a set of well-known authors, books, and active reservations
/// so the application starts with meaningful demonstration data.
/// </summary>
/// <remarks>
/// Author and member IDs are fixed so seed data remains consistent across restarts.
/// Member IDs are intentionally aligned with those produced by the Members service seeder,
/// allowing the two services to reference the same logical people.
/// </remarks>
public sealed class LibrarySeeding : ICanSeedEvents
{
    static readonly AuthorId _tolkienId = new(new Guid(0xa1000000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1));
    static readonly AuthorId _herbertId = new(new Guid(0xa1000000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2));
    static readonly AuthorId _asimovId = new(new Guid(0xa1000000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3));
    static readonly AuthorId _leGuinId = new(new Guid(0xa1000000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4));

    static readonly Guid _aliceId = new(0xb2000000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
    static readonly Guid _bobId = new(0xb2000000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2);

    /// <inheritdoc/>
    public void Seed(IEventSeedingBuilder builder)
    {
        SeedAuthors(builder);
        SeedBooks(builder);
    }

    static void SeedAuthors(IEventSeedingBuilder builder) =>
        builder
            .For(_tolkienId, [new AuthorRegistered("J.R.R. Tolkien")])
            .For(_herbertId, [new AuthorRegistered("Frank Herbert")])
            .For(_asimovId, [new AuthorRegistered("Isaac Asimov")])
            .For(_leGuinId, [new AuthorRegistered("Ursula K. Le Guin")]);

    static void SeedBooks(IEventSeedingBuilder builder) =>
        builder
            .ForEventSource("978-0547928227", [
                new BookAddedToInventory("The Hobbit", _tolkienId, 5),
                new BookReserved("978-0547928227", _aliceId)
            ])
            .For("978-0618346257", [
                new BookAddedToInventory("The Fellowship of the Ring", _tolkienId, 3)
            ])
            .ForEventSource("978-0441013593", [
                new BookAddedToInventory("Dune", _herbertId, 4),
                new BookReserved("978-0441013593", _bobId)
            ])
            .For("978-0553293357", [
                new BookAddedToInventory("Foundation", _asimovId, 3)
            ])
            .For("978-0441478125", [
                new BookAddedToInventory("The Left Hand of Darkness", _leGuinId, 2)
            ]);
}
