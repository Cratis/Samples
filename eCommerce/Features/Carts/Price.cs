namespace eCommerce.Carts;

public record Price(decimal Value) : ConceptAs<decimal>(Value)
{
    public static implicit operator Price(decimal value) => new(value);
    public static implicit operator decimal(Price price) => price.Value;
}
