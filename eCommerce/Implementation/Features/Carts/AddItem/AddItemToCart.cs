// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Applications.ModelBinding;
using Cratis.Chronicle.EventSequences;
using Cratis.Chronicle.Models;
using FluentValidation;

namespace eCommerce.Carts.AddItem;

public record AddItemToCart(
    [FromRoute, ModelKey] CartId CartId,
    Sku Sku,
    Price Price);

[Route("/api/carts/{cartId}/items")]
public class AddItemToCartHandler(IAggregateRootFactory aggregateRootFactory) : ControllerBase
{
    [HttpPost]
    public async Task AddItemToCart([FromRequest] AddItemToCart command)
    {
        var cart = await aggregateRootFactory.Get<Cart>(command.CartId);
        await cart.AddItem(command.Sku, command.Price);
    }
}

public class AddItemToCartRules : RulesFor<AddItemToCartRules, AddItemToCart>
{
    public AddItemToCartRules()
    {
        RuleForState(m => m.NumberOfItems)
            .LessThan(3)
            .WithMessage("You can only add 3 items to the cart");
    }

    public int NumberOfItems { get; set; }

    public override void DefineState(IProjectionBuilderFor<AddItemToCartRules> builder) => builder
        .From<ItemAddedToCart>(_ => _.Count(m => m.NumberOfItems));
}
