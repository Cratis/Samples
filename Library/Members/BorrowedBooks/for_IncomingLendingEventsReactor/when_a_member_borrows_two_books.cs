// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Lending.Contracts.Borrowing;

namespace Members.BorrowedBooks.for_IncomingLendingEventsReactor;

public class when_a_member_borrows_two_books : Specification
{
    const string FirstIsbn = "978-0316389704";
    static readonly MemberId _member = MemberId.New();
    EventForEventSourceId _first = null!;
    EventForEventSourceId _second = null!;
    BookLended _firstLending = null!;
    BookLended _secondLending = null!;

    [Fact]
    public void should_record_each_book_on_its_own_stream() => _first.EventSourceId.ShouldNotEqual(_second.EventSourceId);

    [Fact]
    public void should_key_the_stream_by_member_and_book() => _first.EventSourceId.ShouldEqual((EventSourceId)BorrowedBookId.For(_member, FirstIsbn));

    [Fact]
    public void should_record_the_lending() => _first.Event.ShouldEqual(new BookLendingRecorded(FirstIsbn, "The Last Wish", _member, _firstLending.BorrowedDate, _firstLending.DueDate));

    protected void Establish()
    {
        var borrowed = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        _firstLending = new(FirstIsbn, "The Last Wish", _member, borrowed, borrowed.AddDays(21));
        _secondLending = new("978-0547928227", "The Hobbit", _member, borrowed, borrowed.AddDays(21));
    }

    protected void Because()
    {
        var reactor = new IncomingLendingEventsReactor();
        _first = reactor.On(_firstLending, EventContext.Empty);
        _second = reactor.On(_secondLending, EventContext.Empty);
    }
}
