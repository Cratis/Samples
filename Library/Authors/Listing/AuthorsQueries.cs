// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive.Subjects;
using MongoDB.Driver;

namespace Library.Authors.Listing;

[Route("/api/authors")]
public class AuthorsQueries(IMongoCollection<Author> collection) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<Author>> AllAuthors()
    {
        var result = await collection.FindAsync(_ => true);
        return result.ToList();
    }

    [HttpGet("observe")]
    public ISubject<IEnumerable<Author>> ObserveAllAuthors() =>
        collection.Observe();
}
