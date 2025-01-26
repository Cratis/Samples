namespace Concepts.Books;

public record BootTitle(string Value) : ConceptAs<string>(Value)
{
    public static implicit operator BootTitle(string value) => new(value);
}
