namespace eCommerce.Carts;

public record CartId(Guid Value) : ConceptAs<Guid>(Value)
{
    public static implicit operator CartId(Guid value) => new(value);
    public static implicit operator Guid(CartId cartId) => cartId.Value;
    public static implicit operator EventSourceId(CartId cartId) => new(cartId.Value.ToString());
}
