using MongoDB.Driver;

namespace Quickstart.Common;

public class ReservedBooks(IMongoCollection<ReservedBook> collection)
{
    public IEnumerable<ReservedBook> GetAll() => collection.Find(Builders<ReservedBook>.Filter.Empty).ToList();
}
