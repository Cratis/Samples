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
