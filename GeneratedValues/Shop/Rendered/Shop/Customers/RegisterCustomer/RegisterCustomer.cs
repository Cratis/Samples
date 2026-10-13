// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Authorization;
using Cratis.Arc.Commands.ModelBound;
using Cratis.Arc.Validation;
using Cratis.Chronicle.Events;
using Shop.Common;

namespace Shop.Shop.Customers.RegisterCustomer;

[global::Cratis.Arc.Commands.ModelBound.CommandAttribute]
[global::Cratis.Arc.Authorization.AllowAnonymousAttribute]
public record RegisterCustomer(global::Shop.Common.CustomerName Name)
{
    public (global::Shop.Common.CustomerId, global::Cratis.Chronicle.EventSequences.EventsWithConcurrencyScopes) Handle([global::Cratis.Arc.Commands.ModelBound.GeneratedValueAttribute("customerId")] global::Shop.Common.CustomerId customerId)
    {
        return (customerId, new global::Cratis.Chronicle.EventSequences.EventsWithConcurrencyScopes([new global::Cratis.Chronicle.EventSequences.EventForEventSourceId(customerId, new global::Shop.Shop.Customers.RegisterCustomer.CustomerRegistered(Name))], global::System.Linq.Enumerable.Select(global::System.Linq.Enumerable.Distinct<global::Cratis.Chronicle.Events.EventSourceId>([customerId]), source => new global::System.Collections.Generic.KeyValuePair<global::Cratis.Chronicle.Events.EventSourceId, global::Cratis.Chronicle.EventSequences.Concurrency.ConcurrencyScope>(source, new(global::Cratis.Chronicle.Events.EventSequenceNumber.BeforeFirst, EventSourceId: source)))));
    }
}

/// <summary>
/// The event that occurs when customer registered.
/// </summary>
[global::Cratis.Chronicle.Events.EventTypeAttribute]
public record CustomerRegistered(global::Shop.Common.CustomerName Name);

