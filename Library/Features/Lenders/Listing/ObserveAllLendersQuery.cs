// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive.Subjects;
using MongoDB.Driver;

namespace Library.Lenders.Listing;

[Route("/api/lenders")]
public class ObserveAllLendersQuery(IMongoCollection<Lender> collection) : ControllerBase
{
    [HttpGet("observe")]
    public ISubject<IEnumerable<Lender>> ObserveAllLenders() =>
        collection.Observe();
}
