// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;

namespace CrossStore;

/// <summary>
/// Represents an order identity in the source store.
/// </summary>
/// <param name="Value">The underlying identifier.</param>
public record OrderId(Guid Value) : EventSourceId<Guid>(Value)
{
    /// <summary>
    /// Represents an unset order identity.
    /// </summary>
    public static readonly OrderId NotSet = new(Guid.Empty);

    /// <summary>
    /// Converts a <see cref="Guid"/> to an order identity.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted order identity.</returns>
    public static implicit operator OrderId(Guid value) => new(value);

    /// <summary>
    /// Creates a new order identity.
    /// </summary>
    /// <returns>A new order identity.</returns>
    public static OrderId New() => new(Guid.NewGuid());
}

/// <summary>
/// Represents the target store's identity for a fulfillment order.
/// </summary>
/// <param name="Value">The underlying identifier.</param>
public record FulfillmentOrderId(Guid Value) : EventSourceId<Guid>(Value)
{
    /// <summary>
    /// Represents an unset fulfillment order identity.
    /// </summary>
    public static readonly FulfillmentOrderId NotSet = new(Guid.Empty);

    /// <summary>
    /// Converts a <see cref="Guid"/> to a fulfillment order identity.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted fulfillment order identity.</returns>
    public static implicit operator FulfillmentOrderId(Guid value) => new(value);
}
