namespace Concepts.Books;

public record PublishedDate(DateTime Value) : ConceptAs<DateTime>(Value)
{
    public static implicit operator PublishedDate(DateTime value) => new(value);
}
