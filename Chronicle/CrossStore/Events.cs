// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;

namespace CrossStore;

/// <summary>
/// Records that the orders domain accepted an order.
/// </summary>
/// <param name="Buyer">The source-owned buyer reference.</param>
/// <param name="Sku">The ordered product.</param>
/// <param name="Quantity">The ordered quantity.</param>
[EventType]
public record OrderPlaced(BuyerReference Buyer, ProductSku Sku, OrderQuantity Quantity);

/// <summary>
/// Publishes only the information fulfillment needs from an accepted order.
/// </summary>
/// <param name="Sku">The product to fulfill.</param>
/// <param name="Quantity">The quantity to fulfill.</param>
[EventType]
public record OrderRequestedForFulfillment(ProductSku Sku, OrderQuantity Quantity);

/// <summary>
/// Records the fulfillment domain's local interpretation of an incoming order contract.
/// </summary>
/// <param name="Sku">The target-owned product identifier.</param>
/// <param name="Quantity">The target-owned quantity to fulfill.</param>
[EventType]
public record FulfillmentOrderReceived(FulfillmentSku Sku, UnitsToFulfill Quantity);
