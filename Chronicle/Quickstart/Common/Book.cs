// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;

namespace Quickstart.Common;

#region Snippet:Quickstart-Book
public record Book(Guid Id, string Title, string Author, string ISBN)
#endregion Snippet:Quickstart-Book
{
    #region Snippet:Quickstart-BooksReducer
    /// <summary>
    /// Builds the book from its inventory event.
    /// </summary>
    /// <param name="event">The inventory event.</param>
    /// <param name="current">The unused current book.</param>
    /// <param name="context">The event source identity.</param>
    /// <returns>The book described by the inventory event.</returns>
    public static Task<Book> Reduce(BookAddedToInventory @event, Book? current, EventContext context) =>
        Task.FromResult(new Book(Guid.Parse(context.EventSourceId), @event.Title, @event.Author, @event.ISBN));
    #endregion Snippet:Quickstart-BooksReducer

    public override string ToString() => Title;
}
