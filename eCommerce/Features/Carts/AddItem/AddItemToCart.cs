using Cratis.Applications.ModelBinding;
using Cratis.Chronicle.Models;
using FluentValidation;

namespace eCommerce.Carts.AddItem;

public record AddItemToCart(
    [FromRoute, ModelKey] CartId CartId,
    Sku Sku);

[Route("/api/carts/{cartId}/items")]
public class AddItemToCartHandler(IAggregateRootFactory aggregateRootFactory) : ControllerBase
{
    [HttpPost]
    public async Task AddItemToCart([FromRequest] AddItemToCart command)
    {
        var cart = await aggregateRootFactory.Get<Cart>(command.CartId);
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
