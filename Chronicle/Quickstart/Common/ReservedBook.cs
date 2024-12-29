namespace Quickstart.Common;

public record ReservedBook(Guid Id, Guid UserId, string Title, string User, DateTimeOffset Reserved)
{
    public override string ToString() => Title;
}
