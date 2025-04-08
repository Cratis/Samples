// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.EventSequences;
using Library.Authors;
using Library.Books.Inventory.Listing;

namespace Library.Books.Inventory.Adding;

public record AddBookToInventory(ISBN ISBN, BootTitle Title, AuthorId Author, DateOnly PublishedDate);

[Route("/api/books/inventory")]
public class AddBookToInventoryHandler(IEventLog eventLog) : ControllerBase
{
    [HttpPost("add")]
    public Task AddBookToInventory([FromBody] AddBookToInventory command) =>
        eventLog.Append(
            BookId.New(),
            new BookAddedToInventory(command.ISBN, command.Title, command.Author, command.PublishedDate));
}
