// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Commands.ModelBound;
using Cratis.Chronicle.Events;
using Cratis.Chronicle.EventSequences;
using Cratis.Concepts;
using Shop.Common;

namespace Shop.Customers.Registration;

/// <summary>
/// Represents the name of a customer.
/// </summary>
/// <param name="Value">The underlying text value.</param>
public record CustomerName(string Value) : ConceptAs<string>(Value)
{
    /// <summary>
    /// Converts text to a <see cref="CustomerName"/>.
    /// </summary>
    /// <param name="value">The text to convert.</param>
    public static implicit operator CustomerName(string value) => new(value);
}

/// <summary>
/// Registers a new customer. The server generates the customer's identity; the request carries only the name.
/// </summary>
/// <param name="Name">The name of the customer.</param>
[Command]
public record RegisterCustomer(CustomerName Name)
{
    /// <summary>
    /// Handles the command.
    /// </summary>
    /// <param name="customerId">The generated identity of the new customer.</param>
    /// <returns>The new identity, and the event that records the registration on it.</returns>
    public (CustomerId, EventForEventSourceId) Handle([GeneratedValue("customerId")] CustomerId customerId) =>
        (customerId, new EventForEventSourceId(customerId, new CustomerRegistered(Name)));
}

/// <summary>
/// The event that occurs when a customer is registered.
/// </summary>
/// <param name="Name">The name of the customer.</param>
[EventType]
public record CustomerRegistered(CustomerName Name);
