// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Library.Books.Inventory.Adding;

namespace Library.Books.Inventory.Listing;

public class BookProjection : IProjectionFor<Book>
{
    public void Define(IProjectionBuilderFor<Book> builder) => builder
        .AutoMap()
        .From<BookAddedToInventory>()
        .From<StockChanged>();
}
