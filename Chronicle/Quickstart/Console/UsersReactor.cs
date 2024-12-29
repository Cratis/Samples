#region Snippet:Quickstart-UsersReactor
using Cratis.Chronicle.Events;
using Cratis.Chronicle.Reactors;
using MongoDB.Driver;

namespace Quickstart;

public class UsersReactor : IReactor
{
    public async Task Onboarded(UserOnboarded @event, EventContext context)
    {
        var user = new User(Guid.Parse(context.EventSourceId), @event.Name, @event.Email);
        var collection = Globals.Database.GetCollection<User>("users");
        await collection.InsertOneAsync(user);
    }
}
#endregion Snippet:Quickstart-UsersReactor
