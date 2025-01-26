using Read.Authors;

namespace Api.Authors;

[Route("/api/authors")]
public class Authors : ControllerBase
{
    [HttpPost]
    public Task RegisterAuthor([FromBody] RegisterAuthor command)
    {
        return Task.CompletedTask;
    }

    [HttpGet]
    public Task<IEnumerable<Author>> AllAuthors()
    {
        return Task.FromResult(Enumerable.Empty<Author>());
    }
}

