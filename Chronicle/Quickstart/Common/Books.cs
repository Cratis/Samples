using MongoDB.Driver;

namespace Quickstart.Common;

public class Books(IMongoCollection<Book> collection)
{
    public IEnumerable<Book> GetAll() => collection.Find(Builders<Book>.Filter.Empty).ToList();
}
