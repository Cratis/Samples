// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Commands.ModelBound;

namespace LearningPath.CommandsAndQueries.Carts;

/// <summary>
/// Adds a product to a shopping cart.
/// </summary>
/// <param name="CartId">The cart to add to. Arc uses it as the event source id for the returned event.</param>
/// <param name="Product">The product to add.</param>
/// <param name="Quantity">How many to add.</param>
[Command]
public record AddItemToCart(CartId CartId, ProductName Product, Quantity Quantity)
{
    /// <summary>
    /// Handles the command by deciding which event happened.
    /// </summary>
    /// <returns>The event Arc appends to the cart's event source.</returns>
    public ItemAddedToCart Handle() => new(Product, Quantity);
}
