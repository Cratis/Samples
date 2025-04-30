namespace eCommerce.Carts.AddItem;

[EventType]
public record ItemAddedToCart(
    Sku Sku,
    Price Price);
