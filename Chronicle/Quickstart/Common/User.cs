namespace Quickstart.Common;

#region Snippet:Quickstart-User
public record User(Guid Id, string Name, string Email)
#endregion Snippet:Quickstart-User
{
    public override string ToString() => Name;
}
