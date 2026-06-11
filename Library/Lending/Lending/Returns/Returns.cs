// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Validation;
using Cratis.Chronicle.Projections.ModelBound;
using Cratis.Monads;
using Library.Lending.Borrowing;

namespace Library.Lending.Returns;

/// <summary>
/// Represents overdue book information for a member.
/// </summary>
/// <param name="Id">Unique identifier for this record.</param>
/// <param name="Isbn">The ISBN of the overdue book.</param>
/// <param name="Title">The title of the book.</param>
/// <param name="BorrowedBy">The member who borrowed the book.</param>
/// <param name="DueDate">When the book was due.</param>
/// <param name="DaysOverdue">Number of days overdue.</param>
[ReadModel]
[FromEvent<BookBorrowed>]
public record OverdueBook(
    Guid Id,
    ISBN Isbn,
    string Title,
    MemberId BorrowedBy,
    DateTimeOffset DueDate,
    int DaysOverdue = 0);

/// <summary>
/// Command to return a borrowed book.
/// </summary>
/// <param name="Isbn">The ISBN of the book being returned.</param>
/// <param name="MemberId">The member returning the book.</param>
[Command]
public record ReturnBook(ISBN Isbn, MemberId MemberId)
{
    /// <summary>
    /// Handles the return book command.
    /// </summary>
    /// <returns>A book returned event.</returns>
    public BookReturned Handle()
    {
        var returnedDate = DateTimeOffset.UtcNow;
        return new BookReturned(Isbn, MemberId, returnedDate);
    }
}
