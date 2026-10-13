// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;
using Cratis.Chronicle.Projections;
using Cratis.Chronicle.Projections.ModelBound;

namespace ProjectionStyles;

/// <summary>
/// Builds the order with model-bound projection attributes.
/// </summary>
/// <param name="Id">The order stream identity.</param>
/// <param name="CustomerName">The customer's display name.</param>
/// <param name="IsShipped">Whether a shipment event has occurred.</param>
/// <param name="ItemCount">Add events minus removal events; not the distinct active line count.</param>
/// <param name="LastUpdated">The occurrence time of the last contributing order event, including child events.</param>
/// <param name="Lines">Product-keyed lines with names joined from product events.</param>
[RemovedWith<OrderCancelled>]
public record ModelBoundOrder(
    OrderId Id,
    [SetFrom<OrderPlaced>] CustomerName CustomerName,
    [SetValue<OrderShipped>(true)] bool IsShipped,
    [Increment<ItemAddedToOrder>]
    [Decrement<ItemRemovedFromOrder>] int ItemCount,
    [FromEvery(contextProperty: nameof(EventContext.Occurred))] DateTimeOffset LastUpdated,
    [ChildrenFrom<ItemAddedToOrder>(key: nameof(ItemAddedToOrder.ProductId), identifiedBy: nameof(ModelBoundOrderLine.ProductId))]
    [ChildrenFrom<ItemQuantityAdjusted>(key: nameof(ItemQuantityAdjusted.ProductId), identifiedBy: nameof(ModelBoundOrderLine.ProductId))]
    [RemovedWith<ItemRemovedFromOrder>(key: nameof(ItemRemovedFromOrder.ProductId))] IEnumerable<ModelBoundOrderLine> Lines);

/// <summary>
/// Represents a model-bound line keyed by the product stream identity.
/// </summary>
/// <param name="ProductId">The product identifying the line and its join source.</param>
/// <param name="Quantity">The current quantity.</param>
/// <param name="ProductName">The current name joined from the product stream.</param>
public record ModelBoundOrderLine(
    ProductId ProductId,
    int Quantity,
    [NoAutoMap]
    [Join<ProductRegistered>(on: nameof(ProductId))]
    [Join<ProductRenamed>(on: nameof(ProductId))] ProductName? ProductName);
