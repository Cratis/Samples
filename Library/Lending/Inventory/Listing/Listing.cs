// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.MongoDB;
using Library.Inventory.Adding;
using MongoDB.Driver;

namespace Library.Inventory.Listing;

/// <summary>
/// Represents a book in the inventory.
/// </summary>
/// <param name="Id">The ISBN of the book.</param>
/// <param name="Title">The title of the book.</param>
[ReadModel]
public record Book(ISBN Id, BookTitle Title)
{
    /// <summary>
    /// Observes all books in the inventory, pushing updates when the collection changes.
    /// </summary>
    /// <param name="collection">The MongoDB collection to observe.</param>
    /// <returns>A subject that emits the full book list on each change.</returns>
    public static ISubject<IEnumerable<Book>> ObserveAll(IMongoCollection<Book> collection) =>
        collection.Observe();
}

/// <summary>
/// Defines the projection from <see cref="BookAddedToInventory"/> to <see cref="Book"/>.
/// </summary>
public class BookProjection : IProjectionFor<Book>
{
    /// <inheritdoc/>
    public void Define(IProjectionBuilderFor<Book> builder) => builder
        .AutoMap()
        .From<BookAddedToInventory>();
}
