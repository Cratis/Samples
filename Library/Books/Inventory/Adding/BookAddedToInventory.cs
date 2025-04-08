// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Library.Authors;

namespace Library.Books.Inventory.Adding;

[EventType]
public record BookAddedToInventory(ISBN ISBN, BootTitle Title, AuthorId Author, DateOnly PublishedDate);
