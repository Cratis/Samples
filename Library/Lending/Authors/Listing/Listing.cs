// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.MongoDB;
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
    /// <summary>
    /// Observes all authors, pushing updates when the collection changes.
    /// </summary>
    /// <param name="collection">The MongoDB collection to observe.</param>
    /// <returns>A subject that emits the full author list on each change.</returns>
    public static ISubject<IEnumerable<Author>> ObserveAll(IMongoCollection<Author> collection) =>
        collection.Observe();
}

/// <summary>
/// Defines the projection from <see cref="AuthorRegistered"/> to <see cref="Author"/>.
/// </summary>
public class AuthorProjection : IProjectionFor<Author>
{
    /// <inheritdoc/>
    public void Define(IProjectionBuilderFor<Author> builder) => builder
        .AutoMap()
        .From<AuthorRegistered>();
}
