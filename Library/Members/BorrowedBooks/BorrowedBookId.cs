// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Security.Cryptography;
using System.Text;

namespace Members.BorrowedBooks;

/// <summary>
/// Identifies the borrowing stream for a member and a book.
/// </summary>
/// <param name="Value">The unique identifier for the member and book.</param>
public record BorrowedBookId(Guid Value) : ConceptAs<Guid>(Value)
{
    /// <summary>
    /// Converts a borrowing identifier to its GUID value.
    /// </summary>
    /// <param name="id">The borrowing identifier.</param>
    public static implicit operator Guid(BorrowedBookId id) => id.Value;

    /// <summary>
    /// Converts a borrowing identifier to an event source identifier.
    /// </summary>
    /// <param name="id">The borrowing identifier.</param>
    public static implicit operator EventSourceId(BorrowedBookId id) => new(id.Value.ToString());

    /// <summary>
    /// Gets the deterministic identifier for a member and ISBN, so lending and return events meet on the same stream.
    /// </summary>
    /// <param name="memberId">The member borrowing the book.</param>
    /// <param name="isbn">The ISBN of the book.</param>
    /// <returns>The same identifier for the same member and ISBN.</returns>
    public static BorrowedBookId For(MemberId memberId, string isbn)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes($"{memberId.Value:N}:{isbn}"));
        return new(new Guid(hash.AsSpan(0, 16)));
    }
}
