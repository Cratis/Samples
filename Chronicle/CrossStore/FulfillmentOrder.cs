// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Projections.ModelBound;

namespace CrossStore;

/// <summary>
/// Provides the fulfillment-owned view of an order received from another event store.
/// </summary>
/// <param name="Id">The fulfillment order identity.</param>
/// <param name="Sku">The target-owned product identifier.</param>
/// <param name="Quantity">The target-owned quantity to fulfill.</param>
[FromEvent<FulfillmentOrderReceived>]
public record FulfillmentOrder(FulfillmentOrderId Id, FulfillmentSku Sku, UnitsToFulfill Quantity);
