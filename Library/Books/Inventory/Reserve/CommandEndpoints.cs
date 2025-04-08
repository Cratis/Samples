// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Library.Books.Inventory.Reserve;

[Route("/api/books/reservations")]
public class Reservations : ControllerBase
{
    [HttpPost("reserve")]
    public Task ReserveBook([FromBody] ReserveBook command)
    {
        return Task.CompletedTask;
    }
}
