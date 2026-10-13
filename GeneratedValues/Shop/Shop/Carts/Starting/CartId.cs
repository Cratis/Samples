// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Concepts;

namespace Shop.Carts.Starting;

/// <summary>
/// Represents the identity of a shopping cart.
/// </summary>
/// <param name="Value">The underlying value.</param>
public record CartId(Guid Value) : ConceptAs<Guid>(Value), IGeneratable<CartId>
{
    /// <summary>
    /// Generates a new cart identity.
    /// </summary>
    /// <returns>A new <see cref="CartId"/>.</returns>
    public static CartId New() => new(GenerateValue.Uuid());
}
