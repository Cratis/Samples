using Cratis.Chronicle.Projections;

namespace Quickstart.Common;

public class BooksProjection : IProjectionFor<Book>
{
    public void Define(IProjectionBuilderFor<Book> builder) => builder
        .From<BookAddedToInventory>(from => from.AutoMap());
}
