// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Library.Authors;

namespace Library.Books.Inventory.Adding;

public record AddBookToInventory(ISBN ISBN, BootTitle Title, AuthorId Author, DateOnly PublishedDate);

[Route("/api/books/inventory")]
public class AddBookToInventoryHandler : ControllerBase
{
    [HttpPost("add")]
    public Task AddBookToInventory([FromBody] AddBookToInventory command)
    {
        return Task.CompletedTask;
    }
}
