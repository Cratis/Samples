// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Projections;
using Events.Books;

namespace Read.Books;

public class BookProjection : IProjectionFor<Book>
{
    public void Define(IProjectionBuilderFor<Book> builder) => builder
        .AutoMap()
        .From<BookAddedToCatalog>();
}
