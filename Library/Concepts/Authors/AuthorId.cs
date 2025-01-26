namespace Concepts.Authors;

public record AuthorId(Guid Value) : ConceptAs<Guid>(Value)
{
    public static implicit operator AuthorId(Guid value) => new(value);
}
