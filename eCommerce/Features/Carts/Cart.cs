using eCommerce.Carts.AddItem;

namespace eCommerce.Carts;

public class Cart : AggregateRoot
{
    public Task AddItem(Sku sku, Price price)
    {
        Apply(new ItemAddedToCart(sku, price));
        return Task.CompletedTask;
    }
}
