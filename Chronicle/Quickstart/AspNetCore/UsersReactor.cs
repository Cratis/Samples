// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#region Snippet:Quickstart-AspNetCore-UsersReactor
using Cratis.Chronicle.Events;
using Cratis.Chronicle.Reactors;
using MongoDB.Driver;
using Quickstart.Common;

namespace Quickstart;

public class UsersReactor(IMongoCollection<User> collection) : IReactor
{
    public async Task Onboarded(UserOnboarded @event, EventContext context)
    {
        var user = new User(Guid.Parse(context.EventSourceId), @event.Name, @event.Email);
        await collection.InsertOneAsync(user);
    }
}
#endregion Snippet:Quickstart-AspNetCore-UsersReactor
