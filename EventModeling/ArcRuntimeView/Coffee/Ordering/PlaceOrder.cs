// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Commands.ModelBound;
using Cratis.Chronicle.Events;

namespace ArcRuntimeView.Coffee.Ordering;

/// <summary>
/// Places a coffee order at the counter.
/// </summary>
/// <param name="Id">The order's event source.</param>
/// <param name="Drink">The requested drink.</param>
[Command]
public record PlaceOrder(OrderId Id, string Drink)
{
    /// <summary>
    /// Records the requested drink.
    /// </summary>
    /// <returns>The fact to append to the order stream.</returns>
    public OrderPlaced Handle() => new(Drink);
}

/// <summary>
/// Records a coffee order.
/// </summary>
/// <param name="Drink">The requested drink.</param>
[EventType]
public record OrderPlaced(string Drink);
