// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Validation;
using FluentValidation;
using Library.Inventory.Adding;
using Library.Lending.Borrowing;

namespace Library.Lending.Reserving;

/// <summary>
/// Represents the availability state of a book.
/// </summary>
/// <param name="Id">The ISBN of the book.</param>
/// <param name="AvailableCount">The number of available copies.</param>
[ReadModel]
public record BookAvailability(ISBN Id, int AvailableCount = 0);

/// <summary>
/// Reduces book availability based on inventory additions and reservations.
/// </summary>
public class BookAvailabilityReducer : IReducerFor<BookAvailability>
{
    /// <summary>
    /// Adds the added copies to the availability.
    /// </summary>
    /// <param name="event">The event to apply.</param>
    /// <param name="current">The current state, if any.</param>
    /// <param name="context">The event context.</param>
    /// <returns>The updated state.</returns>
    public BookAvailability OnBookAddedToInventory(BookAddedToInventory @event, BookAvailability? current, EventContext context)
    {
        var state = current ?? new BookAvailability(new ISBN(context.EventSourceId.Value));
        return state with { AvailableCount = state.AvailableCount + @event.Count };
    }

    /// <summary>
    /// Takes a copy out of availability when it is reserved.
    /// </summary>
    /// <param name="event">The event to apply.</param>
    /// <param name="current">The current state, if any.</param>
    /// <param name="context">The event context.</param>
    /// <returns>The updated state.</returns>
    public BookAvailability OnBookReserved(BookReserved @event, BookAvailability? current, EventContext context)
    {
        var state = current ?? new BookAvailability(@event.Isbn);
        return state with { AvailableCount = state.AvailableCount - 1 };
    }

    /// <summary>
    /// Takes a copy out of availability when it is borrowed.
    /// </summary>
    /// <param name="event">The event to apply.</param>
    /// <param name="current">The current state, if any.</param>
    /// <param name="context">The event context.</param>
    /// <returns>The updated state.</returns>
    public BookAvailability OnBookBorrowed(BookBorrowed @event, BookAvailability? current, EventContext context)
    {
        var state = current ?? new BookAvailability(@event.Isbn);
        return state with { AvailableCount = state.AvailableCount - 1 };
    }

    /// <summary>
    /// Puts a copy back into availability when it is returned.
    /// </summary>
    /// <param name="event">The event to apply.</param>
    /// <param name="current">The current state, if any.</param>
    /// <param name="context">The event context.</param>
    /// <returns>The updated state.</returns>
    public BookAvailability OnBookReturned(BookReturned @event, BookAvailability? current, EventContext context)
    {
        var state = current ?? new BookAvailability(@event.Isbn);
        return state with { AvailableCount = state.AvailableCount + 1 };
    }
}

/// <summary>
/// Command to reserve a book for a member.
/// </summary>
/// <param name="Isbn">The ISBN of the book to reserve.</param>
/// <param name="MemberId">The member reserving the book.</param>
[Command]
public record ReserveBook(ISBN Isbn, MemberId MemberId)
{
    /// <summary>
    /// Handles the reserve book command. Whether a copy is available is decided by <see cref="ReserveBookValidator"/>.
    /// </summary>
    /// <returns>A reservation event.</returns>
    public BookReserved Handle() => new(Isbn, MemberId);
}

/// <summary>
/// Refuses to reserve a book when no copy is available. Read protection is On for this project, so Arc reads
/// <see cref="BookAvailability"/> fresh from the event log; if another event for the same book arrives after
/// that read, nothing from the command is written.
/// </summary>
public class ReserveBookValidator : CommandValidator<ReserveBook>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReserveBookValidator"/> class.
    /// </summary>
    /// <param name="availability">The availability of the book being reserved.</param>
    public ReserveBookValidator(BookAvailability? availability) =>
        RuleFor(command => command.Isbn)
            .Must(_ => availability is { AvailableCount: > 0 })
            .WithMessage("No copies of this book are available to reserve.");
}

/// <summary>
/// Event raised when a member reserves a book.
/// </summary>
/// <param name="Isbn">The ISBN of the book.</param>
/// <param name="MemberId">The member who reserved the book.</param>
[EventType]
public record BookReserved(ISBN Isbn, MemberId MemberId);
