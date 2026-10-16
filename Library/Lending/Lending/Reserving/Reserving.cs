// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Validation;
using Cratis.Monads;
using Library.Inventory.Adding;
using Library.Lending.Borrowing;

namespace Library.Lending.Reserving;

/// <summary>
/// Represents the availability state of a book.
/// </summary>
/// <param name="Id">The ISBN of the book.</param>
/// <param name="AvailableCount">The number of available copies.</param>
[ReadModel]
public record BookAvailability(ISBN Id, int AvailableCount = 0)
{
    /// <summary>
    /// Adds the added copies to the availability.
    /// </summary>
    /// <param name="event">The event to apply.</param>
    /// <param name="current">The current state, if any.</param>
    /// <param name="context">The event context.</param>
    /// <returns>The updated state.</returns>
    public static BookAvailability Reduce(BookAddedToInventory @event, BookAvailability? current, EventContext context)
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
    public static BookAvailability Reduce(BookReserved @event, BookAvailability? current, EventContext context)
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
    public static BookAvailability Reduce(BookBorrowed @event, BookAvailability? current, EventContext context)
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
    public static BookAvailability Reduce(BookReturned @event, BookAvailability? current, EventContext context)
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
    /// Handles the reserve book command by checking availability.
    /// </summary>
    /// <param name="availability">The current availability of the book.</param>
    /// <returns>A reservation event if a copy is available, or a validation error.</returns>
    public Result<BookReserved, ValidationResult> Handle(BookAvailability availability)
    {
        if (availability.AvailableCount <= 0)
        {
            return ValidationResult.Error($"No available copies to reserve for book {Isbn}");
        }

        return new BookReserved(Isbn, MemberId);
    }
}

/// <summary>
/// Event raised when a member reserves a book.
/// </summary>
/// <param name="Isbn">The ISBN of the book.</param>
/// <param name="MemberId">The member who reserved the book.</param>
[EventType]
public record BookReserved(ISBN Isbn, MemberId MemberId);
