// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Library.Authors.Registration;
using MongoDB.Driver;

namespace Library.Authors.Listing;

/// <summary>
/// Represents an author.
/// </summary>
/// <param name="Id">The unique identifier of the author.</param>
/// <param name="Name">The name of the author.</param>
[ReadModel]
public record Author(AuthorId Id, AuthorName Name)
{
    public static ISubject<IEnumerable<Author>> AllAuthors(IMongoCollection<Author> collection) =>
        collection.Observe();
}

public class AuthorProjection : IProjectionFor<Author>
{
    public void Define(IProjectionBuilderFor<Author> builder) => builder
        .AutoMap()
        .From<AuthorRegistered>();
}
