// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Testing.ReadModels;

namespace Members.BorrowedBooks.for_borrowed_book_reducer;

public class when_a_member_borrows_two_books : Specification
{
    static readonly MemberId _member = MemberId.New();
    static readonly BorrowedBookId _firstKey = BorrowedBookId.For(_member, "978-0316389704");
    static readonly BorrowedBookId _secondKey = BorrowedBookId.For(_member, "978-0547928227");
    static readonly DateTimeOffset _borrowed = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
    readonly ReadModelScenario<BorrowedBook> _scenario = new();
    AppendedEvent[] _events = [];

    [Fact]
    public void should_keep_both_books() => _scenario.Instances.Count.ShouldEqual(2);

    [Fact]
    public void should_use_the_stream_as_row_id_for_the_first_book() => _scenario.InstanceForEventSourceId(_firstKey)!.Id.ShouldEqual(_firstKey.Value);

    [Fact]
    public void should_keep_the_first_book_isbn() => _scenario.InstanceForEventSourceId(_firstKey)!.Isbn.ShouldEqual("978-0316389704");

    [Fact]
    public void should_keep_the_first_book_title() => _scenario.InstanceForEventSourceId(_firstKey)!.Title.ShouldEqual("The Last Wish");

    [Fact]
    public void should_keep_the_first_book_member() => _scenario.InstanceForEventSourceId(_firstKey)!.BorrowedBy.ShouldEqual(_member);

    [Fact]
    public void should_keep_the_first_book_borrowed_date() => _scenario.InstanceForEventSourceId(_firstKey)!.BorrowedDate.ShouldEqual(_borrowed);

    [Fact]
    public void should_keep_the_first_book_due_date() => _scenario.InstanceForEventSourceId(_firstKey)!.DueDate.ShouldEqual(_borrowed.AddDays(21));

    [Fact]
    public void should_leave_the_first_book_active() => _scenario.InstanceForEventSourceId(_firstKey)!.ReturnedDate.ShouldBeNull();

    [Fact]
    public void should_use_the_stream_as_row_id_for_the_second_book() => _scenario.InstanceForEventSourceId(_secondKey)!.Id.ShouldEqual(_secondKey.Value);

    [Fact]
    public void should_keep_the_second_book_isbn() => _scenario.InstanceForEventSourceId(_secondKey)!.Isbn.ShouldEqual("978-0547928227");

    [Fact]
    public void should_keep_the_second_book_title() => _scenario.InstanceForEventSourceId(_secondKey)!.Title.ShouldEqual("The Hobbit");

    [Fact]
    public void should_keep_the_second_book_member() => _scenario.InstanceForEventSourceId(_secondKey)!.BorrowedBy.ShouldEqual(_member);

    [Fact]
    public void should_keep_the_second_book_borrowed_date() => _scenario.InstanceForEventSourceId(_secondKey)!.BorrowedDate.ShouldEqual(_borrowed);

    [Fact]
    public void should_keep_the_second_book_due_date() => _scenario.InstanceForEventSourceId(_secondKey)!.DueDate.ShouldEqual(_borrowed.AddDays(21));

    [Fact]
    public void should_leave_the_second_book_active() => _scenario.InstanceForEventSourceId(_secondKey)!.ReturnedDate.ShouldBeNull();

    protected void Establish() => _events =
    [
        new(EventContext.EmptyWithEventSourceId(_firstKey) with { SequenceNumber = 1 }, new BookLendingRecorded("978-0316389704", "The Last Wish", _member, _borrowed, _borrowed.AddDays(21))),
        new(EventContext.EmptyWithEventSourceId(_secondKey) with { SequenceNumber = 2 }, new BookLendingRecorded("978-0547928227", "The Hobbit", _member, _borrowed, _borrowed.AddDays(21)))
    ];

    protected async Task Because() => await _scenario.ProcessAppendedEvents(_events);
}
