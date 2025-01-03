using MongoDB.Driver;

namespace Quickstart.Common;

#region Snippet:Quickstart-BorrowedBooks
public class BorrowedBooks(IMongoCollection<BorrowedBook> collection)
{
    public IEnumerable<BorrowedBook> GetAll() => collection.Find(Builders<BorrowedBook>.Filter.Empty).ToList();
}
#endregion Snippet:Quickstart-BorrowedBooks
