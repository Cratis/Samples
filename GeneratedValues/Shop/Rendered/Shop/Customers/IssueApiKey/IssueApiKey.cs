// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Authorization;
using Cratis.Arc.Commands.ModelBound;
using Cratis.Arc.Validation;
using Cratis.Chronicle.Events;
using Shop.Common;

namespace Shop.Shop.Customers.IssueApiKey;

[global::Cratis.Arc.Commands.ModelBound.CommandAttribute]
[global::Cratis.Arc.Authorization.AllowAnonymousAttribute]
public record IssueApiKey(global::Shop.Common.CustomerId CustomerId) : global::Cratis.Chronicle.Events.ICanProvideEventSourceId
{
    /// <inheritdoc/>
    public global::Cratis.Chronicle.Events.EventSourceId GetEventSourceId() => CustomerId;

    public (global::Shop.Common.ApiKey, global::Cratis.Chronicle.EventSequences.EventsWithConcurrencyScopes) Handle([global::Cratis.Arc.Commands.ModelBound.GeneratedValueAttribute("apiKey")] global::Shop.Common.ApiKey apiKey)
    {
        return (apiKey, new global::Cratis.Chronicle.EventSequences.EventsWithConcurrencyScopes([new global::Cratis.Chronicle.EventSequences.EventForEventSourceId(CustomerId, new global::Shop.Shop.Customers.IssueApiKey.ApiKeyIssued())], global::System.Linq.Enumerable.Select(global::System.Linq.Enumerable.Distinct<global::Cratis.Chronicle.Events.EventSourceId>([.. (global::System.String.Equals(CustomerId.Value.ToString(), apiKey.Value, global::System.StringComparison.Ordinal) ? new global::Cratis.Chronicle.Events.EventSourceId[] { CustomerId } : [])]), source => new global::System.Collections.Generic.KeyValuePair<global::Cratis.Chronicle.Events.EventSourceId, global::Cratis.Chronicle.EventSequences.Concurrency.ConcurrencyScope>(source, new(global::Cratis.Chronicle.Events.EventSequenceNumber.BeforeFirst, EventSourceId: source)))));
    }
}

/// <summary>
/// The event that occurs when api key issued.
/// </summary>
[global::Cratis.Chronicle.Events.EventTypeAttribute]
public record ApiKeyIssued();

