namespace Concepts.Books;

public record ISBN(string Value) : ConceptAs<string>(Value)
{
    public static implicit operator ISBN(string value) => new(value);
}
