// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MongoDB.Driver;

namespace Quickstart.Common;

public class OverdueBooks(IMongoCollection<OverdueBook> collection)
{
    public IEnumerable<OverdueBook> GetAll() => collection.Find(Builders<OverdueBook>.Filter.Empty).ToList();
}
