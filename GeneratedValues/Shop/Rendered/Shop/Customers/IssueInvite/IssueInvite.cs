// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Authorization;
using Cratis.Arc.Commands.ModelBound;
using Cratis.Arc.Validation;
using Cratis.Chronicle.Events;
using Shop.Common;

namespace Shop.Shop.Customers.IssueInvite;

[global::Cratis.Arc.Commands.ModelBound.CommandAttribute]
[global::Cratis.Arc.Authorization.AllowAnonymousAttribute]
public record IssueInvite(global::Shop.Common.CustomerId CustomerId) : global::Cratis.Chronicle.Events.ICanProvideEventSourceId
{
    /// <inheritdoc/>
    public global::Cratis.Chronicle.Events.EventSourceId GetEventSourceId() => CustomerId;

    public (global::Shop.Common.InviteCode, global::Cratis.Chronicle.EventSequences.EventsWithConcurrencyScopes) Handle([global::Cratis.Arc.Commands.ModelBound.GeneratedValueAttribute("inviteCode")] global::Shop.Common.InviteCode inviteCode)
    {
        return (inviteCode, new global::Cratis.Chronicle.EventSequences.EventsWithConcurrencyScopes([new global::Cratis.Chronicle.EventSequences.EventForEventSourceId(CustomerId, new global::Shop.Shop.Customers.IssueInvite.InviteIssued(inviteCode))], global::System.Linq.Enumerable.Select(global::System.Linq.Enumerable.Distinct<global::Cratis.Chronicle.Events.EventSourceId>([.. (global::System.String.Equals(CustomerId.Value.ToString(), inviteCode.Value, global::System.StringComparison.Ordinal) ? new global::Cratis.Chronicle.Events.EventSourceId[] { CustomerId } : [])]), source => new global::System.Collections.Generic.KeyValuePair<global::Cratis.Chronicle.Events.EventSourceId, global::Cratis.Chronicle.EventSequences.Concurrency.ConcurrencyScope>(source, new(global::Cratis.Chronicle.Events.EventSequenceNumber.BeforeFirst, EventSourceId: source)))));
    }
}

/// <summary>
/// The event that occurs when invite issued.
/// </summary>
[global::Cratis.Chronicle.Events.EventTypeAttribute]
public record InviteIssued(global::Shop.Common.InviteCode InviteCode);

