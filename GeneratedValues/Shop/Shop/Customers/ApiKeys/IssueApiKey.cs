// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Commands.ModelBound;
using Cratis.Chronicle.Events;
using Cratis.Chronicle.EventSequences;
using Shop.Common;

namespace Shop.Customers.ApiKeys;

/// <summary>
/// Issues a new API key to a customer. The key is returned once and never recorded in the event.
/// </summary>
/// <param name="CustomerId">The customer receiving the key.</param>
[Command]
public record IssueApiKey(CustomerId CustomerId)
{
    /// <summary>
    /// Handles the command.
    /// </summary>
    /// <param name="apiKey">The generated API key.</param>
    /// <returns>The API key, and the event that records that a key was issued.</returns>
    public (ApiKey, EventForEventSourceId) Handle([GeneratedValue("apiKey")] ApiKey apiKey) =>
        (apiKey, new EventForEventSourceId(CustomerId, new ApiKeyIssued()));
}

/// <summary>
/// The event that occurs when a customer is issued an API key. It deliberately carries no key.
/// </summary>
[EventType]
public record ApiKeyIssued;
