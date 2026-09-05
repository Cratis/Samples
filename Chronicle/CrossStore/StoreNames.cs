// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CrossStore;

/// <summary>
/// Names the two event stores hosted by the same Chronicle server.
/// </summary>
public static class StoreNames
{
    /// <summary>
    /// The source event store that owns order placement.
    /// </summary>
    public const string Orders = "CrossStoreOrders";

    /// <summary>
    /// The target event store that owns fulfillment work.
    /// </summary>
    public const string Fulfillment = "CrossStoreFulfillment";
}

/// <summary>
/// Names the target-owned subscription from the orders outbox.
/// </summary>
public static class StoreSubscriptionIds
{
    /// <summary>
    /// The stable identifier for the filtered orders-to-fulfillment subscription.
    /// </summary>
    public const string OrdersForFulfillment = "orders-for-fulfillment";
}
