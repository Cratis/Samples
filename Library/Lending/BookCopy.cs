// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Library;

/// <summary>
/// Represents a unique identifier for an individual copy of a book.
/// </summary>
/// <param name="Value">The unique identifier value.</param>
public record BookCopy(string Value) : EventSourceId(Value)
{
    public static implicit operator BookCopy(string id) => new(id);

    public static implicit operator string(BookCopy copy) => copy.Value;

    public static BookCopy New(ISBN isbn, int copyNumber) => new($"{isbn}-{copyNumber:D4}");
}
