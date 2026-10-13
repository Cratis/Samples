// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Authorization;
using Cratis.Arc.Commands.ModelBound;
using Cratis.Arc.Validation;
using Cratis.Chronicle.Events;
using Shop.Common;

namespace Shop.Shop.Orders.PlaceOrder;

[global::Cratis.Arc.Commands.ModelBound.CommandAttribute]
[global::Cratis.Arc.Authorization.AllowAnonymousAttribute]
public record PlaceOrder(global::Shop.Common.CustomerId CustomerId)
{
    public (global::Shop.Common.OrderId, global::Cratis.Chronicle.EventSequences.EventsWithConcurrencyScopes) Handle([global::Cratis.Arc.Commands.ModelBound.GeneratedValueAttribute("orderId")] global::Shop.Common.OrderId orderId)
    {
        return (orderId, new global::Cratis.Chronicle.EventSequences.EventsWithConcurrencyScopes([new global::Cratis.Chronicle.EventSequences.EventForEventSourceId(orderId, new global::Shop.Shop.Orders.PlaceOrder.OrderPlaced(CustomerId))], global::System.Linq.Enumerable.Select(global::System.Linq.Enumerable.Distinct<global::Cratis.Chronicle.Events.EventSourceId>([orderId]), source => new global::System.Collections.Generic.KeyValuePair<global::Cratis.Chronicle.Events.EventSourceId, global::Cratis.Chronicle.EventSequences.Concurrency.ConcurrencyScope>(source, new(global::Cratis.Chronicle.Events.EventSequenceNumber.BeforeFirst, EventSourceId: source)))));
    }
}

/// <summary>
/// The event that occurs when order placed.
/// </summary>
[global::Cratis.Chronicle.Events.EventTypeAttribute]
public record OrderPlaced(global::Shop.Common.CustomerId CustomerId);

