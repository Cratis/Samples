// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Lending.Contracts.Borrowing;

namespace Members.BorrowedBooks.for_IncomingLendingEventsReactor;

public class when_a_borrowed_book_is_returned : Specification
{
    const string Isbn = "978-0316389704";
    static readonly MemberId _member = MemberId.New();
    BookReturnedToLibrary _return = null!;
    EventForEventSourceId _result = null!;

    [Fact]
    public void should_record_the_return_on_the_books_stream() => _result.EventSourceId.ShouldEqual((EventSourceId)BorrowedBookId.For(_member, Isbn));

    [Fact]
    public void should_record_the_return() => _result.Event.ShouldEqual(new BookReturnRecorded(Isbn, _return.ReturnedDate));

    protected void Establish() => _return = new(Isbn, _member, new DateTimeOffset(2026, 1, 8, 0, 0, 0, TimeSpan.Zero));

    protected void Because() => _result = new IncomingLendingEventsReactor().On(_return, EventContext.Empty);
}
