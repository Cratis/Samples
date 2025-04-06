// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Library.Books.Inventory.Listing;

[Route("/api/books/inventory")]
public class QueryEndpoints : ControllerBase
{
    [HttpGet]
    public Task<IEnumerable<Book>> AllBooks()
    {
        return Task.FromResult(Enumerable.Empty<Book>());
    }
}
