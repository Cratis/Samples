namespace Quickstart.Common;

public record BorrowedBook(Guid Id, Guid UserId, string Title, string User, DateTimeOffset Borrowed, DateTimeOffset Returned)
{
    public override string ToString() => Title;
}
