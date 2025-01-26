// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#region Snippet:Quickstart-BooksReducer
using Cratis.Chronicle.Events;
using Cratis.Chronicle.Reducers;

namespace Quickstart.Common;

public class BooksReducer : IReducerFor<Book>
{
    public Task<Book> Added(BookAddedToInventory @event, Book? initialState, EventContext context) =>
         Task.FromResult(new Book(Guid.Parse(@context.EventSourceId), @event.Title, @event.Author, @event.ISBN));
}
#endregion Snippet:Quickstart-BooksReducer
