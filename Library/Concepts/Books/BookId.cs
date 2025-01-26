namespace Concepts.Books;

public record BookId(Guid Value) : ConceptAs<Guid>(Value)
{
    public static implicit operator BookId(Guid value) => new(value);
}
