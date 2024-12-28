using MongoDB.Driver;

namespace Quickstart.Common;

public class Users(IMongoCollection<User> users)
{
    public IEnumerable<User> GetAll() => users.Find(Builders<User>.Filter.Empty).ToList();
}
