// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Commands.ModelBound;
using Cratis.Chronicle.Events;
using Cratis.Chronicle.EventSequences;
using Cratis.Concepts;
using Shop.Common;

namespace Shop.Orders.Placing;

/// <summary>
/// Represents the identity of an order: a TypeID such as "order_01m4t7txm0fgx9p7tehmp6mprg".
/// </summary>
/// <remarks>
/// The context form lets Arc pass the command's received time, so the time part of the identifier is the
/// moment the command arrived rather than a second read of the wall clock. TypeID has no tenant part.
/// </remarks>
/// <param name="Value">The underlying text value.</param>
public record OrderId(string Value) : EventSourceId<string>(Value), IGeneratableInContext<OrderId>
{
    /// <summary>
    /// Generates a new order identity for the given context.
    /// </summary>
    /// <param name="context">The time and tenant of the operation generating the identity.</param>
    /// <returns>A new <see cref="OrderId"/>.</returns>
    public static OrderId New(GenerationContext context) => new(GenerateValue.TypeId("order", context));
}

/// <summary>
/// Places an order for a customer. The server generates the order's identity.
/// </summary>
/// <param name="CustomerId">The customer placing the order.</param>
[Command]
public record PlaceOrder(CustomerId CustomerId)
{
    /// <summary>
    /// Handles the command.
    /// </summary>
    /// <param name="orderId">The generated identity of the new order.</param>
    /// <returns>The new identity, and the event that records the order on it.</returns>
    public (OrderId, EventForEventSourceId) Handle([GeneratedValue("orderId")] OrderId orderId) =>
        (orderId, new EventForEventSourceId(orderId, new OrderPlaced(CustomerId)));
}

/// <summary>
/// The event that occurs when an order is placed.
/// </summary>
/// <param name="CustomerId">The customer who placed the order.</param>
[EventType]
public record OrderPlaced(CustomerId CustomerId);
