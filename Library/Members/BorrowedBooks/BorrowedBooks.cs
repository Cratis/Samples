// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Identity;
using Cratis.Chronicle.Projections.ModelBound;
using Cratis.Chronicle.Reactors;
using Lending.Contracts.Borrowing;
using Members.Identity;
using MongoDB.Driver;

namespace Members.BorrowedBooks;

/// <summary>
/// Private event recorded locally when a book is lended to a member from the Lending service.
/// This event is produced by consuming the BookLended contract event from the Lending service outbox.
/// </summary>
/// <param name="Isbn">The ISBN of the borrowed book.</param>
/// <param name="Title">The title of the borrowed book.</param>
/// <param name="BorrowedBy">The identifier of the member who borrowed the book.</param>
/// <param name="BorrowedDate">When the book was borrowed.</param>
/// <param name="DueDate">When the book is due to be returned.</param>
[EventType]
public record BookLendingRecorded(string Isbn, string Title, MemberId BorrowedBy, DateTimeOffset BorrowedDate, DateTimeOffset DueDate);

/// <summary>
/// Private event recorded locally when a borrowed book is returned from the Lending service.
/// This event is produced by consuming the BookReturnedToLibrary contract event from the Lending service outbox.
/// </summary>
/// <param name="Isbn">The ISBN of the returned book.</param>
/// <param name="ReturnedDate">When the book was returned.</param>
[EventType]
public record BookReturnRecorded(string Isbn, DateTimeOffset ReturnedDate);

/// <summary>
/// Reduces borrowed book records based on lending and return events.
/// </summary>
public class BorrowedBookReducer : IReducerFor<BorrowedBook>
{
    /// <summary>
    /// Reduces the current borrowed book state based on events.
    /// </summary>
    /// <param name="current">The current state.</param>
    /// <param name="event">The event to apply.</param>
    /// <returns>
    /// The updated state with any applicable changes from the event.
    /// </returns>
    public BorrowedBook Reduce(BorrowedBook current, object @event) => @event switch
    {
        BookReturnRecorded e when e.Isbn == current.Isbn =>
            current with { ReturnedDate = e.ReturnedDate },
        _ => current
    };
}

/// <summary>
/// Represents a single borrowing record, capturing which member borrowed a specific book.
/// </summary>
/// <param name="Id">The unique identifier for this borrowing record.</param>
/// <param name="Isbn">The ISBN of the borrowed book.</param>
/// <param name="Title">The title of the borrowed book.</param>
/// <param name="BorrowedBy">The identifier of the member who borrowed the book.</param>
/// <param name="BorrowedDate">When the book was borrowed.</param>
/// <param name="DueDate">When the book is due to be returned.</param>
/// <param name="ReturnedDate">When the book was returned, if applicable.</param>
[ReadModel]
public record BorrowedBook(
    Guid Id,
    string Isbn,
    string Title,
    MemberId BorrowedBy,
    DateTimeOffset BorrowedDate,
    DateTimeOffset DueDate,
    DateTimeOffset? ReturnedDate = null)
{
    /// <summary>
    /// Observes all books currently borrowed by the authenticated member, pushing updates when the collection changes.
    /// The member is resolved server-side from the authenticated identity — the client never supplies the identifier.
    /// </summary>
    /// <param name="identityProvider">Resolves the current member identity from the active request.</param>
    /// <param name="collection">The MongoDB collection to observe.</param>
    /// <returns>
    /// A subject that emits the member's currently borrowed books (not yet returned) whenever the collection changes.
    /// </returns>
    public static async Task<ISubject<IEnumerable<BorrowedBook>>> GetMyBorrowedBooks(IIdentityProvider identityProvider, IMongoCollection<BorrowedBook> collection)
    {
        var identity = await identityProvider.Get<MemberIdentityDetails>();
        var memberId = identity.Details.MemberId;
        return collection.Observe(book => book.BorrowedBy == memberId && book.ReturnedDate == null);
    }
}

/// <summary>
/// Reactor that consumes book lending and return events from the Lending service outbox
/// and records them as private events in the Members service event log.
/// </summary>
/// <param name="eventStore">The event store for appending private events.</param>
[Reactor]
public class IncomingLendingEventsReactor(IEventStore eventStore) : IReactor
{
    /// <summary>
    /// Handles BookLended events from the Lending service outbox by recording them locally.
    /// </summary>
    /// <param name="event">The book lended contract event.</param>
    /// <param name="context">The event context.</param>
    public async Task On(BookLended @event, EventContext context)
    {
        var memberId = new MemberId(@event.MemberId);
        await eventStore.EventLog.Append(
            @event.MemberId.ToString(),
            new BookLendingRecorded(@event.BookIsbn, @event.BookTitle, memberId, @event.BorrowedDate, @event.DueDate));
    }

    /// <summary>
    /// Handles BookReturnedToLibrary events from the Lending service outbox by recording them locally.
    /// </summary>
    /// <param name="event">The book returned contract event.</param>
    /// <param name="context">The event context.</param>
    public async Task On(BookReturnedToLibrary @event, EventContext context)
    {
        await eventStore.EventLog.Append(
            @event.MemberId.ToString(),
            new BookReturnRecorded(@event.BookIsbn, @event.ReturnedDate));
    }
}
