// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Commands.ModelBound;

namespace Shop.Carts.Starting;

/// <summary>
/// After the migration: Arc generates the identity before calling the handler, and a spec can pin it by the name "cart".
/// </summary>
[Command]
public record StartCart
{
    /// <summary>
    /// Handles the command.
    /// </summary>
    /// <param name="cartId">The generated identity of the new cart.</param>
    /// <returns>The identity of the new cart.</returns>
    public CartId Handle([GeneratedValue("cart")] CartId cartId) => cartId;
}
