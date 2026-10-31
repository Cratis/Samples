// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Validation;
using Library.Inventory.Adding;

namespace Library.Lending.Reserving.for_ReserveBook;

public class when_the_last_copy_is_reserved_concurrently : Specification
{
    static readonly ISBN _isbn = new("978-0316389704");
    readonly CommandScenario<ReserveBook> _scenario = new();
    CommandResult _result = null!;

    [Fact]
    public void should_not_succeed() => _result.ShouldNotBeSuccessful();

    [Fact]
    public void should_report_a_conflict() => _result.ShouldHaveValidationErrorBecauseOf(ValidationResultReason.ConcurrencyViolation);

    [Fact]
    public void should_append_nothing() => _scenario.AppendedEvents.Count.ShouldEqual(0);

    protected void Establish()
    {
        _scenario.Given.ForEventSource(_isbn).Events(new BookAddedToInventory("The Last Wish", AuthorId.New(), 1));
        _scenario.AppendConcurrently(_isbn, new BookReserved(_isbn, MemberId.New()));
    }

    protected async Task Because() => _result = await _scenario.Execute(new ReserveBook(_isbn, MemberId.New()));

    protected void Destroy() => _scenario.Dispose();
}
