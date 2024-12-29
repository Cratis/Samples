namespace Quickstart.Common;

#region Snippet:Quickstart-BorrowedBook
public record BorrowedBook(Guid Id, Guid UserId, string Title, string User, DateTimeOffset Borrowed, DateTimeOffset Returned)
#endregion Snippet:Quickstart-BorrowedBook
{
    public override string ToString() => Title;
}
