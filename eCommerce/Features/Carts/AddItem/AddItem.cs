// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Applications.Commands;
using Cratis.Chronicle.Keys;
using FluentValidation;

namespace eCommerce.Carts.AddItem;

[Command]
public record AddItemToCart(
    [Key] CartId CartId,
    Sku Sku,
    Price Price) : ICanProvideEventSourceId
{
    public EventSourceId GetEventSourceId() => Guid.Empty;

    public ItemAddedToCart Handle() => new(Sku, Price);
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

[EventType]
public record ItemAddedToCart(Sku Sku, Price Price);
