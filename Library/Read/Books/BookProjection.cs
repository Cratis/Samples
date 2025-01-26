using Cratis.Chronicle.Projections;
using Events.Books;

namespace Read.Books;

public class BookProjection : IProjectionFor<Book>
{
    public void Define(IProjectionBuilderFor<Book> builder) => builder
        .AutoMap()
        .From<BookAddedToCatalog>();
}
