// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Testing.ReadModels;
using Library.Inventory.Adding;

namespace Library.Inventory.Listing.for_book_projection;

public class when_a_book_is_added_to_inventory
{
    [Fact(Skip = "ReadModelScenario currently fails for fluent projections in this sample; kept as pending coverage.")]
    public async Task should_use_event_source_as_read_model_id()
    {
        var isbn = (ISBN)"978-0316389704";
        var scenario = new ReadModelScenario<Book>();
        await scenario.Given
            .ForEventSource(isbn)
            .Events(new BookAddedToInventory("The Last Wish", AuthorId.New(), 3));

        scenario.Instance!.Id.ShouldEqual(isbn);
    }

    [Fact(Skip = "ReadModelScenario currently fails for fluent projections in this sample; kept as pending coverage.")]
    public async Task should_map_the_book_title()
    {
        var scenario = new ReadModelScenario<Book>();
        await scenario.Given
            .ForEventSource((ISBN)"978-0316389704")
            .Events(new BookAddedToInventory("The Last Wish", AuthorId.New(), 3));

        scenario.Instance!.Title.Value.ShouldEqual("The Last Wish");
    }
}
