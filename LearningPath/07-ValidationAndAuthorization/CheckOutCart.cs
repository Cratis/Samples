// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Authorization;
using Cratis.Arc.Commands.ModelBound;
using Cratis.Arc.Validation;
using FluentValidation;

namespace LearningPath.ValidationAndAuthorization.Carts;

/// <summary>
/// Checks out a shopping cart.
/// </summary>
/// <param name="CartId">The cart to check out.</param>
[Command]
[Roles(nameof(CartRole.Customer))]
public record CheckOutCart(CartId CartId)
{
    /// <summary>
    /// Handles the command by deciding which event happened. The state rule lives in <see cref="CheckOutCartValidator"/>.
    /// </summary>
    /// <returns>The event to append.</returns>
    public CartCheckedOut Handle() => new();
}

/// <summary>
/// Refuses the command once the cart has been checked out. Read protection is On for this project, so Arc reads
/// <see cref="CartSummary"/> fresh from the event log and nothing is written if the cart changes after that read.
/// </summary>
public class CheckOutCartValidator : CommandValidator<CheckOutCart>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CheckOutCartValidator"/> class.
    /// </summary>
    /// <param name="cart">The cart state, or null when the cart has no events yet; an absent cart is open.</param>
    public CheckOutCartValidator(CartSummary? cart) =>
        RuleFor(command => command.CartId)
            .Must(_ => cart is not { CheckedOut: true })
            .WithMessage("Cart has already been checked out.");
}
