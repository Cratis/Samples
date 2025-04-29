using eCommerce.Carts.AddItem;
using eCommerce.Carts.RemoveItem;
using MongoDB.Driver;

namespace eCommerce.Carts.Contents;

public record Cart(CartId Id, IEnumerable<CartItem> Items, Price TotalPrice);

public record CartItem(Sku Sku, Price Price);

public class CartProjection : IProjectionFor<Cart>
{
    public void Define(IProjectionBuilderFor<Cart> builder) => builder
        .AutoMap()
        .From<ItemAddedToCart>(e => e.Add(m => m.TotalPrice).With(e => e.Price))
        .Children(c => c.Items, cartBuilder => cartBuilder
            .From<ItemAddedToCart>()
            .RemovedWith<ItemRemovedFromCart>());
}

[Route("/api/carts/{cartId}")]
public class CartQueries(IMongoCollection<Cart> collection) : ControllerBase
{
    [HttpGet]
    public ISubject<Cart> GetCart([FromRoute] CartId cartId) => collection.ObserveById(cartId);
}
