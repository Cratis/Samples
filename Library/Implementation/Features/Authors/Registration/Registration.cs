// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Applications.ModelBinding;
using Cratis.Chronicle.Events.Constraints;
using Cratis.Chronicle.EventSequences;
using Cratis.Chronicle.Models;
using Cratis.Chronicle.Transactions;
using Cratis.Execution;

namespace Library.Authors.Registration;

public record RegisterAuthor(AuthorName Name);

[Route("/api/authors/register")]
public class RegisterAuthorHandler(IEventLog eventLog) : ControllerBase
{
    [HttpPost]
    public async Task Register([FromRequest] RegisterAuthor command)
    {
        var authorId = Guid.NewGuid();
        await eventLog.Transactional.Append(authorId, new AuthorRegistered(command.Name), "Author");
    }
}

[EventType]
public record AuthorRegistered(AuthorName Name);

public class UniqueAuthorName : IConstraint
{
    public void Define(IConstraintBuilder builder) => builder
        .Unique(_ => _
            .On<AuthorRegistered>(e => e.Name)
            .WithMessage("Author name must be unique"));
}
