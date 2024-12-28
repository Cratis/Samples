using Cratis.Chronicle.Projections;

namespace Quickstart.Common;

public class BorrowedBooksProjection : IProjectionFor<BorrowedBook>
{
    public void Define(IProjectionBuilderFor<BorrowedBook> builder) => builder
        .From<BookBorrowed>(from => from
            .Set(m => m.UserId).To(e => e.UserId)
            .Set(m => m.Borrowed).ToEventContextProperty(c => c.Occurred))
        .Join<BookAddedToInventory>(bookBuilder => bookBuilder
            .On(m => m.Id)
            .Set(m => m.Title).To(e => e.Title))
        .Join<UserOnboarded>(userBuilder => userBuilder
            .On(m => m.UserId)
            .Set(m => m.User).To(e => e.Name));
}
