// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;

namespace ProjectionStyles;

/// <summary>
/// Represents an order folded from its own stream, with a derived distinct line count.
/// </summary>
/// <remarks>
/// The distinct line count is derived from the next state; projection counters count events, not keyed children.
/// This stream-local fold cannot join product events from other streams.
/// </remarks>
/// <param name="Id">The order stream identity.</param>
/// <param name="CustomerName">The customer's display name.</param>
/// <param name="IsShipped">Whether a shipment event has occurred.</param>
/// <param name="ItemCount">The number of distinct active lines.</param>
/// <param name="LastUpdated">The occurrence time of the last handled order event.</param>
/// <param name="Lines">Product-keyed lines; product names remain absent without a cross-stream join.</param>
public record ReducedOrder(OrderId Id, CustomerName CustomerName, bool IsShipped, int ItemCount, DateTimeOffset LastUpdated, IEnumerable<OrderLine> Lines)
{
    /// <summary>
    /// Initializes the order from its placement event.
    /// </summary>
    /// <param name="event">The placement.</param>
    /// <param name="current">The unused prior state.</param>
    /// <param name="context">The event identity and occurrence time.</param>
    /// <returns>The initialized order.</returns>
    public static ReducedOrder Reduce(OrderPlaced @event, ReducedOrder? current, EventContext context) =>
        new(new OrderId(Guid.Parse(context.EventSourceId)), @event.CustomerName, false, 0, context.Occurred, []);

    /// <summary>
    /// Upserts one product line instead of appending a duplicate.
    /// </summary>
    /// <param name="event">The line and its replacement quantity.</param>
    /// <param name="current">The current order, or null before placement or after cancellation.</param>
    /// <param name="context">The event occurrence time.</param>
    /// <returns>The next order, or null if no order exists.</returns>
    public static ReducedOrder? Reduce(ItemAddedToOrder @event, ReducedOrder? current, EventContext context) => current is null
        ? null
        : WithLines(current, current.Lines.Where(line => line.ProductId != @event.ProductId).Append(new(@event.ProductId, @event.Quantity, null)), context);

    /// <summary>
    /// Replaces the quantity of an existing product line.
    /// </summary>
    /// <param name="event">The adjustment.</param>
    /// <param name="current">The current order, or null if absent.</param>
    /// <param name="context">The event occurrence time.</param>
    /// <returns>The next order, or null if absent.</returns>
    public static ReducedOrder? Reduce(ItemQuantityAdjusted @event, ReducedOrder? current, EventContext context) => current is null
        ? null
        : WithLines(current, current.Lines.Select(line => line.ProductId == @event.ProductId ? line with { Quantity = @event.Quantity } : line), context);

    /// <summary>
    /// Removes a line and recalculates the count from the remaining lines.
    /// </summary>
    /// <param name="event">The removal.</param>
    /// <param name="current">The current order, or null if absent.</param>
    /// <param name="context">The event occurrence time.</param>
    /// <returns>The next order, or null if absent.</returns>
    public static ReducedOrder? Reduce(ItemRemovedFromOrder @event, ReducedOrder? current, EventContext context) => current is null
        ? null
        : WithLines(current, current.Lines.Where(line => line.ProductId != @event.ProductId), context);

    /// <summary>
    /// Marks an existing order shipped.
    /// </summary>
    /// <param name="event">The shipment.</param>
    /// <param name="current">The current order, or null if absent.</param>
    /// <param name="context">The event occurrence time.</param>
    /// <returns>The shipped order, or null if absent.</returns>
    public static ReducedOrder? Reduce(OrderShipped @event, ReducedOrder? current, EventContext context) =>
        current is null ? null : current with { IsShipped = true, LastUpdated = context.Occurred };

    /// <summary>
    /// Deletes the order by returning null.
    /// </summary>
    /// <param name="event">The cancellation.</param>
    /// <param name="current">The unused prior order.</param>
    /// <returns>Null, which removes the read model.</returns>
    public static ReducedOrder? Reduce(OrderCancelled @event, ReducedOrder? current) => null;

    static ReducedOrder WithLines(ReducedOrder current, IEnumerable<OrderLine> lines, EventContext context)
    {
        var nextLines = lines.ToArray();
        return current with { Lines = nextLines, ItemCount = nextLines.Length, LastUpdated = context.Occurred };
    }
}
