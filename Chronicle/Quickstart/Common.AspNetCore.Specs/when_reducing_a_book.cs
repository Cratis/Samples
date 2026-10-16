// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;
using Xunit;

namespace Quickstart.Common.AspNetCore.Specs;

public class when_reducing_a_book
{
    [Fact]
    public async Task should_create_the_book_from_the_event_and_source_identity()
    {
        var id = Guid.NewGuid();
        var context = EventContext.Empty with { EventSourceId = id.ToString() };
        var result = await Book.Reduce(new BookAddedToInventory("The Last Wish", "Andrzej Sapkowski", "978-0316389704"), null, context);

        Assert.Equal(new Book(id, "The Last Wish", "Andrzej Sapkowski", "978-0316389704"), result);
    }

    [Fact]
    public async Task should_replace_the_current_book_without_mutating_it()
    {
        var current = new Book(Guid.NewGuid(), "Old title", "Old author", "Old ISBN");
        var context = EventContext.Empty with { EventSourceId = current.Id.ToString() };
        var result = await Book.Reduce(new BookAddedToInventory("New title", "New author", "New ISBN"), current, context);

        Assert.Equal(new Book(current.Id, "New title", "New author", "New ISBN"), result);
        Assert.Equal("Old title", current.Title);
    }
}
