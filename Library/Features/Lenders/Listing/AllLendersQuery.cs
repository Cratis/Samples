// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MongoDB.Driver;

namespace Library.Lenders.Listing;

[Route("/api/lenders")]
public class AllLendersQuery(IMongoCollection<Lender> collection) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<Lender>> AllLenders()
    {
        var result = await collection.FindAsync(_ => true);
        return await result.ToListAsync();
    }
}
