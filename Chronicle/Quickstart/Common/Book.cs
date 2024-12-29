namespace Quickstart.Common;

#region Snippet:Quickstart-Book
public record Book(Guid Id, string Title, string Author, string ISBN)
#endregion Snippet:Quickstart-Book
{
    public override string ToString() => Title;
}
