// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Library.Inventory.Adding;
using MongoDB.Driver;

namespace Library.Inventory.Listing;

public record Book(ISBN Id, BookTitle Title);

public class BookProjection : IProjectionFor<Book>
{
    public void Define(IProjectionBuilderFor<Book> builder) => builder
        .AutoMap()
        .From<BookAddedToInventory>();
}

[Route("/api/books")]
public class AuthorQueries(IMongoCollection<Book> collection) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<Book>> GetAll()
    {
        var result = await collection.FindAsync(_ => true);
        return result.ToList();
    }

    [HttpGet("observe")]
    public ISubject<IEnumerable<Book>> ObserveAll() =>
        collection.Observe();
}
