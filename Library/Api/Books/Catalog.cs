using Read.Books;

namespace Api.Books;

[Route("/api/books/catalog")]
public class Catalog : ControllerBase
{
    [HttpGet]
    public Task<IEnumerable<Book>> AllBooks()
    {
        return Task.FromResult(Enumerable.Empty<Book>());
    }

    public Task RegisterBook([FromBody] RegisterBook command)
    {
        return Task.CompletedTask;
    }
}
