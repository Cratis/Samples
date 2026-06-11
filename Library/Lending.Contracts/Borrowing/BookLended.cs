// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Lending.Contracts.Borrowing;

/// <summary>
/// Public contract event published to the outbox when a member borrows a book.
/// Consumed by the Members service to track borrowed books.
/// </summary>
/// <param name="BookIsbn">The ISBN of the borrowed book.</param>
/// <param name="BookTitle">The title of the borrowed book.</param>
/// <param name="MemberId">The unique identifier of the member who borrowed the book.</param>
/// <param name="BorrowedDate">When the book was borrowed.</param>
/// <param name="DueDate">When the book is due to be returned.</param>
[EventType]
public record BookLended(
    string BookIsbn,
    string BookTitle,
    Guid MemberId,
    DateTimeOffset BorrowedDate,
    DateTimeOffset DueDate);
