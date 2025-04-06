// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.EventSequences;
using Cratis.Chronicle.Transactions;

namespace Library.Authors.Registration;

public record RegisterAuthor(AuthorId AuthorId, AuthorName Name);

[Route("/api/authors")]
public class RegisterAuthorHandler(IEventLog eventLog) : ControllerBase
{
    [HttpPost("register")]
    public Task RegisterAuthor([FromBody] RegisterAuthor command)
    {
        eventLog.Append(
            command.AuthorId.Value,
            new AuthorRegistered(command.Name));

        return Task.CompletedTask;
    }
}
