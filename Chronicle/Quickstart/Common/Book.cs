namespace Quickstart.Common;

public record Book(Guid Id, string Title, string Author, string ISBN)
{
    public override string ToString() => Title;
}
