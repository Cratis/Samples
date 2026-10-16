// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Library.Inventory.Adding.for_AddBookTitleToInventory;

public class when_adding_a_book_title : Specification
{
    readonly CommandScenario<AddBookTitleToInventory> _scenario = new();
    CommandResult _result;

    async Task Because() => _result = await _scenario.Execute(new AddBookTitleToInventory("The Last Wish", "978-0316389704", AuthorId.New(), 2));

    void Destroy() => _scenario.Dispose();

    [Fact] void should_succeed() => _result.ShouldBeSuccessful();
    [Fact] void should_append_the_inventory_event() => _scenario.AppendedEvents.Count.ShouldEqual(1);
}
