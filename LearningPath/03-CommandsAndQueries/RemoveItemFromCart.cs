// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Commands.ModelBound;

namespace LearningPath.CommandsAndQueries.Carts;

/// <summary>
/// Removes a product from a shopping cart.
/// </summary>
/// <param name="CartId">The cart to remove from.</param>
/// <param name="Product">The product to remove.</param>
/// <param name="Quantity">How many to remove.</param>
[Command]
public record RemoveItemFromCart(CartId CartId, ProductName Product, Quantity Quantity)
{
    /// <summary>
    /// Handles the command by deciding which event happened.
    /// </summary>
    /// <returns>The event Arc appends to the cart's event source.</returns>
    public ItemRemovedFromCart Handle() => new(Product, Quantity);
}
