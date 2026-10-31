// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Testing.ReadModels;

namespace Members.BorrowedBooks.for_borrowed_book_reducer;

public class when_applying_lending_and_return_events
{
    static readonly DateTimeOffset _borrowed = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
    static readonly DateTimeOffset _due = _borrowed.AddDays(21);
    static readonly DateTimeOffset _returned = _borrowed.AddDays(7);

    [Fact]
    public async Task should_create_the_borrowed_book_when_lending_is_recorded()
    {
        var member = MemberId.New();
        var scenario = new ReadModelScenario<BorrowedBook>();
        await scenario.Given
            .ForEventSource(BorrowedBookId.For(member, "978-0316389704"))
            .Events(new BookLendingRecorded("978-0316389704", "The Last Wish", member, _borrowed, _due));

        scenario.Instance!.Title.ShouldEqual("The Last Wish");
        scenario.Instance.BorrowedBy.ShouldEqual(member);
        scenario.Instance.DueDate.ShouldEqual(_due);
        scenario.Instance.ReturnedDate.ShouldBeNull();
    }

    [Fact]
    public async Task should_set_returned_date_when_return_is_recorded()
    {
        var member = MemberId.New();
        var scenario = new ReadModelScenario<BorrowedBook>();
        await scenario.Given
            .ForEventSource(BorrowedBookId.For(member, "978-0316389704"))
            .Events(
                new BookLendingRecorded("978-0316389704", "The Last Wish", member, _borrowed, _due),
                new BookReturnRecorded("978-0316389704", _returned));

        scenario.Instance!.ReturnedDate.ShouldEqual(_returned);
    }
}
