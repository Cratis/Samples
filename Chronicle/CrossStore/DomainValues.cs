// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Concepts;

namespace CrossStore;

/// <summary>
/// Represents a source-owned buyer reference.
/// </summary>
/// <param name="Value">The underlying reference.</param>
public record BuyerReference(string Value) : ConceptAs<string>(Value)
{
    /// <summary>
    /// Represents an unset buyer reference.
    /// </summary>
    public static readonly BuyerReference NotSet = new(string.Empty);

    /// <summary>
    /// Converts a string to a buyer reference.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted buyer reference.</returns>
    public static implicit operator BuyerReference(string value) => new(value);
}

/// <summary>
/// Represents a product identifier in the orders contract.
/// </summary>
/// <param name="Value">The underlying stock-keeping unit.</param>
public record ProductSku(string Value) : ConceptAs<string>(Value)
{
    /// <summary>
    /// Represents an unset product identifier.
    /// </summary>
    public static readonly ProductSku NotSet = new(string.Empty);

    /// <summary>
    /// Converts a string to a product identifier.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted product identifier.</returns>
    public static implicit operator ProductSku(string value) => new(value);
}

/// <summary>
/// Represents the number of units requested by an order.
/// </summary>
/// <param name="Value">The underlying quantity.</param>
public record OrderQuantity(int Value) : ConceptAs<int>(Value)
{
    /// <summary>
    /// Represents an unset order quantity.
    /// </summary>
    public static readonly OrderQuantity NotSet = new(0);

    /// <summary>
    /// Converts an integer to an order quantity.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted order quantity.</returns>
    public static implicit operator OrderQuantity(int value) => new(value);
}

/// <summary>
/// Represents a target-owned product identifier used during fulfillment.
/// </summary>
/// <param name="Value">The underlying stock-keeping unit.</param>
public record FulfillmentSku(string Value) : ConceptAs<string>(Value)
{
    /// <summary>
    /// Represents an unset fulfillment product identifier.
    /// </summary>
    public static readonly FulfillmentSku NotSet = new(string.Empty);

    /// <summary>
    /// Converts a string to a fulfillment product identifier.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted fulfillment product identifier.</returns>
    public static implicit operator FulfillmentSku(string value) => new(value);
}

/// <summary>
/// Represents the target-owned number of units to fulfill.
/// </summary>
/// <param name="Value">The underlying quantity.</param>
public record UnitsToFulfill(int Value) : ConceptAs<int>(Value)
{
    /// <summary>
    /// Represents an unset fulfillment quantity.
    /// </summary>
    public static readonly UnitsToFulfill NotSet = new(0);

    /// <summary>
    /// Converts an integer to a fulfillment quantity.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted fulfillment quantity.</returns>
    public static implicit operator UnitsToFulfill(int value) => new(value);
}
