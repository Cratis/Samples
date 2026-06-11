// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Validation;
using Cratis.Chronicle.Projections.ModelBound;
using Cratis.Chronicle.Reactors;
using Cratis.Monads;
using Lending.Contracts.Borrowing;

namespace Library.Lending.Borrowing;

/// <summary>
/// Represents a borrowed book record for a member.
/// </summary>
/// <param name="Id">Unique identifier for this borrow record.</param>
/// <param name="Isbn">The ISBN of the borrowed book.</param>
/// <param name="Title">The title of the borrowed book.</param>
/// <param name="BorrowedBy">The member who borrowed the book.</param>
/// <param name="BorrowedDate">When the book was borrowed.</param>
/// <param name="DueDate">When the book is due to be returned.</param>
[ReadModel]
[FromEvent<BookBorrowed>]
public record MemberBorrowedBook(
    Guid Id,
    ISBN Isbn,
    string Title,
    MemberId BorrowedBy,
    DateTimeOffset BorrowedDate,
    DateTimeOffset DueDate);

/// <summary>
/// Command to borrow a book for a member.
/// </summary>
/// <param name="Isbn">The ISBN of the book to borrow.</param>
/// <param name="MemberId">The member borrowing the book.</param>
/// <param name="BorrowDays">The number of days the member can borrow the book (default: 21 days).</param>
[Command]
public record BorrowBook(ISBN Isbn, MemberId MemberId, int BorrowDays = 21)
{
    /// <summary>
    /// Handles the borrow book command by validating availability.
    /// </summary>
    /// <param name="availability">The current availability of the book.</param>
    /// <returns>A book borrowed event if a copy is available, or a validation error.</returns>
    public Result<BookBorrowed, ValidationResult> Handle(Reserving.BookAvailability availability)
    {
        if (availability.AvailableCount <= 0)
        {
            return ValidationResult.Error($"No available copies of book {Isbn} to borrow");
        }

        var borrowedDate = DateTimeOffset.UtcNow;
        var dueDate = borrowedDate.AddDays(BorrowDays);

        return new BookBorrowed(Isbn, "Book Title", MemberId, borrowedDate, dueDate);
    }
}

/// <summary>
/// Event raised when a member borrows a book.
/// </summary>
/// <param name="Isbn">The ISBN of the borrowed book.</param>
/// <param name="Title">The title of the borrowed book.</param>
/// <param name="MemberId">The member who borrowed the book.</param>
/// <param name="BorrowedDate">When the book was borrowed.</param>
/// <param name="DueDate">When the book is due to be returned.</param>
[EventType]
public record BookBorrowed(ISBN Isbn, string Title, MemberId MemberId, DateTimeOffset BorrowedDate, DateTimeOffset DueDate);

/// <summary>
/// Event raised when a borrowed book is returned.
/// </summary>
/// <param name="Isbn">The ISBN of the returned book.</param>
/// <param name="MemberId">The member who returned the book.</param>
/// <param name="ReturnedDate">When the book was returned.</param>
[EventType]
public record BookReturned(ISBN Isbn, MemberId MemberId, DateTimeOffset ReturnedDate);

/// <summary>
/// Publishes borrowing and return events to the outbox for consumption by other services.
/// </summary>
/// <param name="eventStore">The event store for appending to outbox.</param>
[Reactor]
public class BorrowingOutboxReactor(IEventStore eventStore) : IReactor
{
    /// <summary>
    /// Publishes book borrowed events to the outbox so that other services (e.g., Members) can be notified.
    /// </summary>
    /// <param name="event">The book borrowed event.</param>
    /// <param name="context">The event context.</param>
    public async Task On(BookBorrowed @event, EventContext context) =>
        await eventStore.GetEventSequence(EventSequenceId.Outbox)
            .Append(context.EventSourceId, new BookLended(
                @event.Isbn,
                @event.Title,
                @event.MemberId,
                @event.BorrowedDate,
                @event.DueDate));

    /// <summary>
    /// Publishes book returned events to the outbox so that other services (e.g., Members) can be notified.
    /// </summary>
    /// <param name="event">The book returned event.</param>
    /// <param name="context">The event context.</param>
    public async Task On(BookReturned @event, EventContext context) =>
        await eventStore.GetEventSequence(EventSequenceId.Outbox)
            .Append(context.EventSourceId, new BookReturnedToLibrary(
                @event.Isbn,
                @event.MemberId,
                @event.ReturnedDate));
}
