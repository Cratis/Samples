// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Applications.ModelBinding;
using Cratis.Chronicle.Keys;
using Cratis.Execution;

namespace Library.Authors.Registration;

/// <summary>
/// Represents a command to register a new author.
/// </summary>
/// <param name="Name">The name of the author to register.</param>
[Command]
public record RegisterAuthor(AuthorName Name)
{
    public (AuthorId, AuthorRegistered) Handle()
    {
        var authorId = AuthorId.New();
        return (authorId, new(Name));
    }
}


// public record RenameAuthor([Key]AuthorId Id, AuthorName NewName)
// {
//     public AuthorRenamed Handle() => new(NewName);
// }

/// <summary>
/// Represents an event that occurs when a new author is registered.
/// </summary>
/// <param name="Name">The name of the author.</param>
[EventType]
public record AuthorRegistered(AuthorName Name);

/// <summary>
/// Holds the constraint that author names must be unique.
/// </summary>
public class UniqueAuthorName : IConstraint
{
    public void Define(IConstraintBuilder builder) => builder
        .Unique(_ => _
            .On<AuthorRegistered>(e => e.Name)
            .WithMessage("Author name must be unique"));
}
