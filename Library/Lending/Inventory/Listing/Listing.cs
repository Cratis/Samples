// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Library.Inventory.Adding;

namespace Library.Inventory.Listing;

public record Book(ISBN Id, BookTitle Title);

public class BookProjection : IProjectionFor<Book>
{
    public void Define(IProjectionBuilderFor<Book> builder) => builder
        .AutoMap()
        .From<BookAddedToInventory>();
}

[Route("/api/books")]
public class BookQueries(IEventStore eventStore) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<Book>> GetAll() =>
        await eventStore.ReadModels.Materialized.GetInstances<Book>();

    [HttpGet("observe")]
    public IObservable<IEnumerable<Book>> ObserveAll() =>
        eventStore.ReadModels.Materialized.ObserveInstances<Book>();
}
