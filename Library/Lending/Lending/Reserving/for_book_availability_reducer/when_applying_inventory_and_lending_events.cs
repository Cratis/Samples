// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Testing.ReadModels;
using Library.Inventory.Adding;
using Library.Lending.Borrowing;

namespace Library.Lending.Reserving.for_book_availability_reducer;

public class when_applying_inventory_and_lending_events
{
    static readonly ISBN _isbn = new("978-0316389704");

    [Fact]
    public async Task should_apply_inventory_reserve_borrow_and_return()
    {
        var member = MemberId.New();
        var scenario = new ReadModelScenario<BookAvailability>();
        await scenario.Given
            .ForEventSource(_isbn)
            .Events(
                new BookAddedToInventory("The Last Wish", AuthorId.New(), 5),
                new BookReserved(_isbn, member),
                new BookBorrowed(_isbn, "The Last Wish", member, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(14)),
                new BookReturned(_isbn, member, DateTimeOffset.UtcNow));

        scenario.Instance!.AvailableCount.ShouldEqual(4);
    }

    [Fact]
    public async Task should_apply_inventory_count()
    {
        var scenario = new ReadModelScenario<BookAvailability>();
        await scenario.Given
            .ForEventSource(_isbn)
            .Events(new BookAddedToInventory("The Last Wish", AuthorId.New(), 3));

        scenario.Instance!.AvailableCount.ShouldEqual(3);
    }
}
