using MongoDB.Driver;

namespace Quickstart.Common;

public class BorrowedBooks(IMongoCollection<BorrowedBook> collection)
{
    public IEnumerable<BorrowedBook> GetAll() => collection.Find(Builders<BorrowedBook>.Filter.Empty).ToList();
}
