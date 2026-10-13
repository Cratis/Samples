// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;

namespace ProjectionStyles;

/// <summary>
/// Records the customer placing an order on the order stream.
/// </summary>
/// <param name="CustomerName">The customer's display name.</param>
[EventType]
public record OrderPlaced(CustomerName CustomerName);

/// <summary>
/// Adds or replaces a product's line on the order stream.
/// </summary>
/// <param name="ProductId">The product identifying the line.</param>
/// <param name="Quantity">The new quantity, not a delta.</param>
[EventType]
public record ItemAddedToOrder(ProductId ProductId, int Quantity);

/// <summary>
/// Replaces a line's quantity on the order stream.
/// </summary>
/// <param name="ProductId">The product identifying the line.</param>
/// <param name="Quantity">The replacement quantity.</param>
[EventType]
public record ItemQuantityAdjusted(ProductId ProductId, int Quantity);

/// <summary>
/// Removes a product's line from the order stream.
/// </summary>
/// <param name="ProductId">The product identifying the line.</param>
[EventType]
public record ItemRemovedFromOrder(ProductId ProductId);

/// <summary>
/// Records shipment on the order stream.
/// </summary>
[EventType]
public record OrderShipped;

/// <summary>
/// Cancels the order and removes its read model.
/// </summary>
[EventType]
public record OrderCancelled;

/// <summary>
/// Registers a name on the product stream, not the order stream.
/// </summary>
/// <param name="ProductName">The product's display name.</param>
[EventType]
public record ProductRegistered(ProductName ProductName);

/// <summary>
/// Updates a name on the product stream, not the order stream.
/// </summary>
/// <param name="ProductName">The replacement display name.</param>
[EventType]
public record ProductRenamed(ProductName ProductName);
