// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;

namespace ProjectionStyles;

/// <summary>
/// Identifies an order event stream.
/// </summary>
/// <param name="Value">The underlying identifier.</param>
public record OrderId(Guid Value) : EventSourceId<Guid>(Value)
{
    /// <summary>
    /// Creates an order stream identity.
    /// </summary>
    /// <returns>A fresh order identity.</returns>
    public static OrderId New() => new(Guid.NewGuid());
}
