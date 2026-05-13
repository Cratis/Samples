// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Seeding;

namespace Quickstart.Common;

/// <summary>
/// Seeds the Quickstart event store with initial users and books.
/// </summary>
public class QuickstartSeeding : ICanSeedEvents
{
    static readonly Guid _janeDoeId = new("a1000000-0000-0000-0000-000000000001");
    static readonly Guid _johnDoeId = new("a1000000-0000-0000-0000-000000000002");
    static readonly Guid _book1Id = new("b1000000-0000-0000-0000-000000000001");
    static readonly Guid _book2Id = new("b1000000-0000-0000-0000-000000000002");

    /// <inheritdoc/>
    public void Seed(IEventSeedingBuilder builder)
    {
        builder
            .For<UserOnboarded>(_janeDoeId, [new("Jane Doe", "jane@interwebs.net")])
            .For<UserOnboarded>(_johnDoeId, [new("John Doe", "john@interwebs.net")])
            .For<BookAddedToInventory>(_book1Id, [new("Metaprogramming in C#: Automate your .NET development and simplify overcomplicated code", "Einar Ingebrigtsen", "978-1837635429")])
            .For<BookAddedToInventory>(_book2Id, [new("Understanding Eventsourcing: Planning and Implementing scalable Systems with Eventmodeling and Eventsourcing", "Martin Dilger", "979-8300933043")]);
    }
}
