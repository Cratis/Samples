// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.EventSequences;
using Cratis.Chronicle.Reactors;

namespace CrossStore;

/// <summary>
/// Translates the source-owned contract into a fulfillment-owned fact.
/// </summary>
/// <remarks>
/// The reactor observes only the orders inbox. Its returned event is appended to the target event log
/// with the incoming event source identity, where target-owned projections can consume it.
/// </remarks>
[Reactor]
[EventSequence(EventSequenceId.InboxPrefix + StoreNames.Orders)]
public class FulfillmentTranslationReactor : IReactor
{
    /// <summary>
    /// Translates an incoming order request without leaking the source domain model into fulfillment.
    /// </summary>
    /// <param name="event">The source-owned contract fact.</param>
    /// <returns>The target-owned local fact.</returns>
    public FulfillmentOrderReceived Requested(OrderRequestedForFulfillment @event) =>
        new(@event.Sku.Value, @event.Quantity.Value);
}
