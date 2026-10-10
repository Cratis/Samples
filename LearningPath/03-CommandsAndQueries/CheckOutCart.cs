// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Commands.ModelBound;

namespace LearningPath.CommandsAndQueries.Carts;

/// <summary>
/// Checks out a shopping cart.
/// </summary>
/// <param name="CartId">The cart to check out.</param>
[Command]
public record CheckOutCart(CartId CartId)
{
    /// <summary>
    /// Handles the command by deciding which event happened.
    /// </summary>
    /// <returns>The event Arc appends to the cart's event source.</returns>
    public CartCheckedOut Handle() => new();
}
