// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Projections;

namespace ProjectionStyles;

/// <summary>
/// Represents an order built by a separate fluent projection.
/// </summary>
/// <param name="Id">The order stream identity.</param>
/// <param name="CustomerName">The customer's display name.</param>
/// <param name="IsShipped">Whether a shipment event has occurred.</param>
/// <param name="ItemCount">Add events minus removal events; not the distinct active line count.</param>
/// <param name="LastUpdated">The occurrence time of the last contributing order event.</param>
/// <param name="Lines">Product-keyed lines with joined product names.</param>
public record FluentOrder(OrderId Id, CustomerName CustomerName, bool IsShipped, int ItemCount, DateTimeOffset LastUpdated, IEnumerable<OrderLine> Lines);

/// <summary>
/// Defines keyed children, event joins, counters, timestamps, and order removal.
/// </summary>
public class FluentOrderProjection : IProjectionFor<FluentOrder>
{
    /// <inheritdoc/>
    public void Define(IProjectionBuilderFor<FluentOrder> builder) => builder
        .From<OrderPlaced>()
        .From<OrderShipped>(from => from.Set(order => order.IsShipped).ToValue(true))
        .From<ItemAddedToOrder>(from => from.Increment(order => order.ItemCount))
        .From<ItemRemovedFromOrder>(from => from.Decrement(order => order.ItemCount))
        .FromEvery(every => every.Set(order => order.LastUpdated).ToEventContextProperty(context => context.Occurred))
        .Children(order => order.Lines, children => children
            .IdentifiedBy(line => line.ProductId)
            .From<ItemAddedToOrder>(from => from.UsingKey(@event => @event.ProductId))
            .From<ItemQuantityAdjusted>(from => from.UsingKey(@event => @event.ProductId))
            .Join<ProductRegistered>()
            .Join<ProductRenamed>()
            .RemovedWith<ItemRemovedFromOrder>(removed => removed.UsingKey(@event => @event.ProductId)))
        .RemovedWith<OrderCancelled>();
}
