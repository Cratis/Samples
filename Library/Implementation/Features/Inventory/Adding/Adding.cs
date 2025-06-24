// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.EventSequences;
using FluentValidation;
using SharpCompress.Archives;

namespace Library.Inventory.Adding;

public record AddBookTitleToInventory(BookTitle Title, ISBN ISBN, AuthorId AuthorId, int Count);

[Route("api/inventory/add")]
public class AddBookTitleToInventoryHandler(IEventLog eventLog) : ControllerBase
{
    [HttpPost]
    public Task AddBookTitleToInventory(AddBookTitleToInventory command) =>
        eventLog.Transactional.Append((string)command.ISBN, new BookAddedToInventory(command.Title, command.AuthorId, command.Count));
}

[EventType]
public record BookAddedToInventory(BookTitle Title, AuthorId Author, int Count);
