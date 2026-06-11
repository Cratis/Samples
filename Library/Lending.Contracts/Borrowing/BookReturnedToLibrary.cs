// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Lending.Contracts.Borrowing;

/// <summary>
/// Public contract event published to the outbox when a member returns a borrowed book.
/// Consumed by the Members service to update borrowed books tracking.
/// </summary>
/// <param name="BookIsbn">The ISBN of the returned book.</param>
/// <param name="MemberId">The unique identifier of the member who returned the book.</param>
/// <param name="ReturnedDate">When the book was returned.</param>
[EventType]
public record BookReturnedToLibrary(
    string BookIsbn,
    Guid MemberId,
    DateTimeOffset ReturnedDate);
