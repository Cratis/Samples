// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Library.Authors;
using Library.Books.Inventory.Adding;

namespace Library.Books.Inventory.Listing;

public record Book(BookId Id, ISBN ISBN, BootTitle Title, AuthorName Author, StockCount StockCount, DateOnly PublishedDate);
