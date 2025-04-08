// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive.Subjects;
using MongoDB.Driver;

namespace Library.Books.Inventory.Listing;

[Route("/api/books/inventory")]
public class ObserveAllBooksQuery(IMongoCollection<Book> collection) : ControllerBase
{
    [HttpGet("observe")]
    public ISubject<IEnumerable<Book>> ObserveAllBooks() => collection.Observe();
}
