namespace eCommerce.Carts;

public record Sku(string Value) : ConceptAs<string>(Value)
{
    public static implicit operator Sku(string value) => new(value);
    public static implicit operator string(Sku price) => price.Value;
}
