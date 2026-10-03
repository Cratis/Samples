// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Commands.ModelBound;
using Cratis.Chronicle.Events;

namespace ArcRuntimeView.Coffee.Preparing;

/// <summary>
/// Records that the barista prepared an order.
/// </summary>
/// <param name="Id">The order's event source.</param>
/// <param name="PreparedAt">The recorded preparation time.</param>
[Command]
public record PrepareOrder(OrderId Id, DateTimeOffset PreparedAt)
{
    /// <summary>
    /// Records the preparation time.
    /// </summary>
    /// <returns>The preparation fact.</returns>
    public OrderPrepared Handle() => new(PreparedAt);
}

/// <summary>
/// Records when an order was prepared.
/// </summary>
/// <param name="PreparedAt">The preparation time.</param>
[EventType]
public record OrderPrepared(DateTimeOffset PreparedAt);
