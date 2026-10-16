// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Authorization;
using Cratis.Arc.Commands.ModelBound;
using Cratis.Arc.Validation;
using FluentValidation;

namespace LearningPath.ValidationAndAuthorization.Carts;

/// <summary>
/// Adds a product to a shopping cart.
/// </summary>
/// <param name="CartId">The cart to add to. Arc uses it as the event source id for the returned event.</param>
/// <param name="Product">The product to add.</param>
/// <param name="Quantity">How many to add.</param>
[Command]
[Authorize]
public record AddItemToCart(CartId CartId, ProductName Product, Quantity Quantity)
{
    /// <summary>
    /// Handles the command by deciding which event happened. The state rule lives in <see cref="AddItemToCartValidator"/>.
    /// </summary>
    /// <returns>The event to append.</returns>
    public ItemAddedToCart Handle() => new(Product, Quantity);
}

/// <summary>
/// Refuses the command once the cart has been checked out. Read protection is On for this project, so Arc reads
/// <see cref="CartSummary"/> fresh from the event log and nothing is written if the cart changes after that read.
/// </summary>
public class AddItemToCartValidator : CommandValidator<AddItemToCart>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddItemToCartValidator"/> class.
    /// </summary>
    /// <param name="cart">The cart state, or null when the cart has no events yet; an absent cart is open.</param>
    public AddItemToCartValidator(CartSummary? cart) =>
        RuleFor(command => command.CartId)
            .Must(_ => cart is not { CheckedOut: true })
            .WithMessage("Cannot add items after checkout.");
}
