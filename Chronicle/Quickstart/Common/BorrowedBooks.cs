// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MongoDB.Driver;

namespace Quickstart.Common;

public class BorrowedBooks(IMongoCollection<BorrowedBook> collection)
{
    public IEnumerable<BorrowedBook> GetAll() => collection.Find(Builders<BorrowedBook>.Filter.Empty).ToList();
}
