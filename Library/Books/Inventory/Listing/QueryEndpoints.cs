// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive.Subjects;
using MongoDB.Driver;

namespace Library.Books.Inventory.Listing;

[Route("/api/books/inventory")]
public class QueryEndpoints(IMongoCollection<Book> collection) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<Book>> AllBooks()
    {
        var result = await collection.FindAsync(_ => true);
        return result.ToList();
    }

    [HttpGet("observe")]
    public ISubject<IEnumerable<Book>> ObserveAllBooks() => collection.Observe();
}
