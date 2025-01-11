using MongoDB.Driver;

namespace Quickstart.Common;

#region Snippet:Quickstart-Books
public class Books(IMongoCollection<Book> collection)
{
    public IEnumerable<Book> GetAll() => collection.Find(Builders<Book>.Filter.Empty).ToList();
}
#endregion Snippet:Quickstart-Books
