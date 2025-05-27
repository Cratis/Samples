// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Library.Authors.Registration;
using MongoDB.Driver;

namespace Library.Authors.Listing;

public record Author(AuthorId Id, AuthorName Name);

public class AuthorProjection : IProjectionFor<Author>
{
    public void Define(IProjectionBuilderFor<Author> builder) => builder
        .AutoMap()
        .From<AuthorRegistered>();
}

[Route("/api/authors")]
public class AuthorQueries(IMongoCollection<Author> collection) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<Author>> GetAll()
    {
        var result = await collection.FindAsync(_ => true);
        return result.ToList();
    }

    [HttpGet("observe")]
    public ISubject<IEnumerable<Author>> ObserveAll() =>
        collection.Observe();
}
