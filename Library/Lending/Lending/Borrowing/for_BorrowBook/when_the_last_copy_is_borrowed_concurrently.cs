// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Validation;
using Library.Inventory.Adding;

namespace Library.Lending.Borrowing.for_BorrowBook;

public class when_the_last_copy_is_borrowed_concurrently : Specification
{
    static readonly ISBN _isbn = new("978-0316389704");
    readonly CommandScenario<BorrowBook> _scenario = new();
    CommandResult _result;

    void Establish()
    {
        _scenario.Given.ForEventSource(_isbn).Events(new BookAddedToInventory("The Last Wish", AuthorId.New(), 1));
        _scenario.AppendConcurrently(_isbn, new BookBorrowed(_isbn, "The Last Wish", MemberId.New(), DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(21)));
    }

    async Task Because() => _result = await _scenario.Execute(new BorrowBook(_isbn, MemberId.New()));

    void Destroy() => _scenario.Dispose();

    [Fact] void should_not_succeed() => _result.ShouldNotBeSuccessful();
    [Fact] void should_report_a_conflict() => _result.ShouldHaveValidationErrorBecauseOf(ValidationResultReason.ConcurrencyViolation);
    [Fact] void should_append_nothing() => _scenario.AppendedEvents.Count.ShouldEqual(0);
}
