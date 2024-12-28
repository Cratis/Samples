namespace Quickstart.Common;

public record User(Guid Id, string Name, string Email)
{
    public override string ToString() => Name;
}
