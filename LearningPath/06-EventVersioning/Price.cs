// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Concepts;

namespace LearningPath.EventVersioning.Carts;

/// <summary>
/// Represents the unit price recorded when a product is added to a cart.
/// </summary>
/// <param name="Value">The price amount.</param>
public record Price(decimal Value) : ConceptAs<decimal>(Value)
{
    /// <summary>
    /// Gets the sentinel used when the event did not record a price, not a claim that the product was free.
    /// </summary>
    public static readonly Price NotRecorded = new(0m);

    /// <summary>
    /// Converts an amount to a price.
    /// </summary>
    /// <param name="value">The amount to convert.</param>
    /// <returns>The price.</returns>
    public static implicit operator Price(decimal value) => new(value);
}
