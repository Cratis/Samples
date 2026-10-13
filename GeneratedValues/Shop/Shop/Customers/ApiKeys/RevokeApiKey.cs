// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Commands.ModelBound;
using Cratis.Chronicle.Events;
using Shop.Common;

namespace Shop.Customers.ApiKeys;

/// <summary>
/// Revokes an API key a customer presents. The key is request input here, so the <see cref="ApiKey"/>
/// concept's <see cref="Cratis.Arc.Chronicle.Commands.NotAuditedAttribute"/> marking keeps it off the causation chain of <see cref="ApiKeyRevoked"/>.
/// </summary>
/// <param name="CustomerId">The customer owning the key.</param>
/// <param name="ApiKey">The key to revoke.</param>
[Command]
public record RevokeApiKey(CustomerId CustomerId, ApiKey ApiKey) : ICanProvideEventSourceId
{
    /// <inheritdoc/>
    public EventSourceId GetEventSourceId() => CustomerId;

    /// <summary>
    /// Handles the command.
    /// </summary>
    /// <returns>The event that records the revocation.</returns>
    public ApiKeyRevoked Handle() => new();
}

/// <summary>
/// The event that occurs when a customer's API key is revoked. It deliberately carries no key.
/// </summary>
[EventType]
public record ApiKeyRevoked;
