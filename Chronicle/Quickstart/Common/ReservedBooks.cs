// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MongoDB.Driver;

namespace Quickstart.Common;

public class ReservedBooks(IMongoCollection<ReservedBook> collection)
{
    public IEnumerable<ReservedBook> GetAll() => collection.Find(Builders<ReservedBook>.Filter.Empty).ToList();
}
