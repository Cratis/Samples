// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CrossStore;

/// <summary>
/// Describes the source-side order input.
/// </summary>
/// <param name="Buyer">The source-owned buyer reference.</param>
/// <param name="Sku">The ordered product.</param>
/// <param name="Quantity">The ordered quantity.</param>
public record PlaceOrderRequest(string Buyer, string Sku, int Quantity);

/// <summary>
/// Describes an accepted asynchronous cross-store flow.
/// </summary>
/// <param name="OrderId">The shared correlation identity carried by event context.</param>
/// <param name="SourceEventStore">The event store that owns order placement.</param>
/// <param name="TargetEventStore">The event store that owns fulfillment.</param>
public record PlaceOrderAccepted(Guid OrderId, string SourceEventStore, string TargetEventStore);
