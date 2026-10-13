// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;

namespace ProjectionStyles;

/// <summary>
/// Identifies a product event stream and its line within an order.
/// </summary>
/// <param name="Value">The underlying identifier.</param>
public record ProductId(Guid Value) : EventSourceId<Guid>(Value)
{
    /// <summary>
    /// Creates a product stream identity.
    /// </summary>
    /// <returns>A fresh product identity.</returns>
    public static ProductId New() => new(Guid.NewGuid());
}
