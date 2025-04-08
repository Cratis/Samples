// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive.Subjects;
using MongoDB.Driver;

namespace Library.Authors.Listing;

[Route("/api/authors")]
public class ObserveAllAuthorsQuery(IMongoCollection<Author> collection) : ControllerBase
{
    [HttpGet("observe")]
    public ISubject<IEnumerable<Author>> ObserveAllAuthors() =>
        collection.Observe();
}
