// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Authorization;
using Cratis.Arc.Commands.ModelBound;
using Cratis.Arc.Validation;
using FluentValidation;

namespace LearningPath.ValidationAndAuthorization.Carts;

/// <summary>
/// Removes a product from a shopping cart.
/// </summary>
/// <param name="CartId">The cart to remove from.</param>
/// <param name="Product">The product to remove.</param>
/// <param name="Quantity">How many to remove.</param>
[Command]
[Authorize]
public record RemoveItemFromCart(CartId CartId, ProductName Product, Quantity Quantity)
{
    /// <summary>
    /// Handles the command by deciding which event happened. The state rule lives in <see cref="RemoveItemFromCartValidator"/>.
    /// </summary>
    /// <returns>The event to append.</returns>
    public ItemRemovedFromCart Handle() => new(Product, Quantity);
}

/// <summary>
/// Refuses the command once the cart has been checked out. Read protection is On for this project, so Arc reads
/// <see cref="CartSummary"/> fresh from the event log and nothing is written if the cart changes after that read.
/// </summary>
public class RemoveItemFromCartValidator : CommandValidator<RemoveItemFromCart>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveItemFromCartValidator"/> class.
    /// </summary>
    /// <param name="cart">The cart state, or null when the cart has no events yet; an absent cart is open.</param>
    public RemoveItemFromCartValidator(CartSummary? cart) =>
        RuleFor(command => command.CartId)
            .Must(_ => cart is not { CheckedOut: true })
            .WithMessage("Cannot remove items after checkout.");
}
