// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Library.Authors.Registration;
using Library.Books.Inventory.Adding;

namespace Library.Books.Inventory.Listing;

public class BookProjection : IProjectionFor<Book>
{
    public void Define(IProjectionBuilderFor<Book> builder) => builder
        .AutoMap()
        .From<BookAddedToInventory>()
        .Join<AuthorRegistered>(authorBuilder => authorBuilder
            .On(m => m.AuthorId)
            .Set(m => m.AuthorName).To(e => e.Name))
        .From<StockChanged>();
}
