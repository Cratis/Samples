using MongoDB.Driver;

namespace Quickstart.Common;

public class OverdueBooks(IMongoCollection<OverdueBook> collection)
{
    public IEnumerable<OverdueBook> GetAll() => collection.Find(Builders<OverdueBook>.Filter.Empty).ToList();
}
