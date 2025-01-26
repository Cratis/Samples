// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
