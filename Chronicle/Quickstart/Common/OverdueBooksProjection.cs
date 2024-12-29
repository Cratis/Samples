using Cratis.Chronicle.Projections;

namespace Quickstart.Common;

public class OverdueBooksProjection : IProjectionFor<OverdueBook>
{
    public void Define(IProjectionBuilderFor<OverdueBook> builder) => builder
        .From<BookOverdue>(from => from.Set(m => m.Id).ToEventContextProperty(c => c.EventSourceId))
        .Join<BookBorrowed>(bookBuilder => bookBuilder
            .On(m => m.Id)
            .Set(m => m.UserId).To(e => e.UserId)
            .Set(m => m.Borrowed).ToEventContextProperty(c => c.Occurred))
        .Join<BookAddedToInventory>(bookBuilder => bookBuilder
            .On(m => m.Id)
            .Set(m => m.Title).To(e => e.Title))
        .Join<UserOnboarded>(userBuilder => userBuilder
            .On(m => m.UserId)
            .Set(m => m.User).To(e => e.Name))
        .RemovedWith<BookReturned>();
}
