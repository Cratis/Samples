// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MongoDB.Driver;

namespace Quickstart.Common;

public class Users(IMongoCollection<User> users)
{
    public IEnumerable<User> GetAll() => users.Find(Builders<User>.Filter.Empty).ToList();
}
