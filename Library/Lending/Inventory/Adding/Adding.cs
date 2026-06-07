// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Library.Inventory.Adding;

[Command]
public record AddBookTitleToInventory(BookTitle Title, ISBN ISBN, AuthorId AuthorId, int Count)
{
    public BookAddedToInventory Handle() => new(Title, AuthorId, Count);
}

[EventType]
public record BookAddedToInventory(BookTitle Title, AuthorId Author, int Count);
