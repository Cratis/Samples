namespace Api.Books;

[Route("/api/books/reservations")]
public class Reservations : ControllerBase
{
    [HttpPost]
    public Task ReserveBook([FromBody] ReserveBook command)
    {
        return Task.CompletedTask;
    }
}
