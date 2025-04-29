
using FluentValidation;

namespace eCommerce.Carts.AddItem;

public record AddItemToCart(Sku Sku);

[Route("/api/carts/{cartId}/items")]
public class AddItemToCartHandler(IAggregateRootFactory aggregateRootFactory) : ControllerBase
{
    [HttpPost]
    public async Task AddItemToCart([FromRoute] CartId cartId, [FromBody] AddItemToCart command)
    {
        var cart = await aggregateRootFactory.Get<Cart>(cartId);
        await cart.AddItem(command.Sku, 42);
    }
}

public class AddItemToCartRules : RulesFor<AddItemToCartRules, AddItemToCart>
{
    public AddItemToCartRules()
    {
        RuleForState(m => m.NumberOfItems)
            .LessThan(3)
            .WithMessage("You can't have more than 3 items in the cart");
    }

    public int NumberOfItems { get; set; }

    public override void DefineState(IProjectionBuilderFor<AddItemToCartRules> builder) => builder
        .From<ItemAddedToCart>(_ => _.Count(m => m.NumberOfItems));
}
