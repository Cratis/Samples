// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Library.Books.Inventory.Adding;

[Route("/api/books/inventory")]
public class CommandEndpoints : ControllerBase
{
    [HttpPost("add")]
    public Task AddBookToInventory([FromBody] AddBookToInventory command)
    {
        return Task.CompletedTask;
    }
}
