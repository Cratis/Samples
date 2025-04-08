// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.EventSequences;

namespace Library.Authors.Registration;

public record RegisterAuthor(AuthorName Name);

[Route("/api/authors")]
public class RegisterAuthorHandler(IEventLog eventLog) : ControllerBase
{
    [HttpPost("register")]
    public Task RegisterAuthor([FromBody] RegisterAuthor command)
    {
        eventLog.Append(
            AuthorId.New(),
            new AuthorRegistered(command.Name));

        return Task.CompletedTask;
    }
}
