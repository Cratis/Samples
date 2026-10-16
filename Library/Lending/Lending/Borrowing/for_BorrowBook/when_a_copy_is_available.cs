// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Library.Inventory.Adding;

namespace Library.Lending.Borrowing.for_BorrowBook;

public class when_a_copy_is_available : Specification
{
    static readonly ISBN _isbn = new("978-0316389704");
    readonly CommandScenario<BorrowBook> _scenario = new();
    CommandResult _result;

    void Establish() => _scenario.Given.ForEventSource(_isbn).Events(new BookAddedToInventory("The Last Wish", AuthorId.New(), 1));

    async Task Because() => _result = await _scenario.Execute(new BorrowBook(_isbn, MemberId.New()));

    void Destroy() => _scenario.Dispose();

    [Fact] void should_succeed() => _result.ShouldBeSuccessful();
    [Fact] void should_append_the_borrow() => _scenario.AppendedEvents.Count.ShouldEqual(1);
}
