// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Library.Inventory.Adding;

namespace Library.Lending.Borrowing.for_BorrowBook;

public class when_no_copy_is_available : Specification
{
    static readonly ISBN _isbn = new("978-0316389704");
    readonly CommandScenario<BorrowBook> _scenario = new();
    CommandResult _result;

    void Establish()
    {
        var member = MemberId.New();
        _scenario.Given.ForEventSource(_isbn).Events(
            new BookAddedToInventory("The Last Wish", AuthorId.New(), 1),
            new BookBorrowed(_isbn, "The Last Wish", member, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(21)));
    }

    async Task Because() => _result = await _scenario.Execute(new BorrowBook(_isbn, MemberId.New()));

    void Destroy() => _scenario.Dispose();

    [Fact] void should_not_succeed() => _result.ShouldNotBeSuccessful();
    [Fact] void should_have_validation_errors() => _result.ShouldHaveValidationErrors();
    [Fact] void should_append_nothing() => _scenario.AppendedEvents.Count.ShouldEqual(0);
}
