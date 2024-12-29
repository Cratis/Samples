using Cratis.Chronicle.Projections;

namespace Quickstart.Common;

public class ReservedBooksProjection : IProjectionFor<ReservedBook>
{
    public void Define(IProjectionBuilderFor<ReservedBook> builder) => builder
        .From<BookReservationPlaced>(from => from
            .Set(m => m.Reserved).ToEventContextProperty(c => c.Occurred)
            .Set(m => m.UserId).To(e => e.UserId))
        .Join<BookAddedToInventory>(bookBuilder => bookBuilder
            .On(m => m.Id)
            .Set(m => m.Title).To(e => e.Title))
        .Join<UserOnboarded>(userBuilder => userBuilder
            .On(m => m.UserId)
            .Set(m => m.User).To(e => e.Name))
        .RemovedWith<BookReservationCancelled>();
}
