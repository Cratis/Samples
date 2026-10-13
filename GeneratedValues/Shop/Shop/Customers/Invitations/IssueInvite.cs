// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Commands.ModelBound;
using Cratis.Chronicle.Events;
using Cratis.Chronicle.EventSequences;
using Cratis.Concepts;
using Shop.Common;

namespace Shop.Customers.Invitations;

/// <summary>
/// Represents a short invite code a person can read aloud or type: 10 characters without look-alikes such as 0/O or 1/I.
/// </summary>
/// <remarks>
/// 10 characters from a 32-character alphabet carry 50 random bits. That is too few for an identifier,
/// so the code is a generated property of a command that targets an existing customer.
/// </remarks>
/// <param name="Value">The underlying text value.</param>
public record InviteCode(string Value) : ConceptAs<string>(Value), IGeneratable<InviteCode>
{
    /// <summary>
    /// The alphabet the code is drawn from.
    /// </summary>
    public const string Alphabet = "23456789ABCDEFGHJKLMNPQRSTUVWXYZ";

    /// <summary>
    /// Generates a new invite code.
    /// </summary>
    /// <returns>A new <see cref="InviteCode"/>.</returns>
    public static InviteCode New() => new(GenerateValue.NanoId(10, Alphabet));
}

/// <summary>
/// Issues an invite code that an existing customer can share.
/// </summary>
/// <param name="CustomerId">The customer issuing the invite.</param>
[Command]
public record IssueInvite(CustomerId CustomerId)
{
    /// <summary>
    /// Handles the command.
    /// </summary>
    /// <param name="inviteCode">The generated invite code.</param>
    /// <returns>The invite code, and the event that records it on the customer.</returns>
    public (InviteCode, EventForEventSourceId) Handle([GeneratedValue("inviteCode")] InviteCode inviteCode) =>
        (inviteCode, new EventForEventSourceId(CustomerId, new InviteIssued(inviteCode)));
}

/// <summary>
/// The event that occurs when a customer issues an invite code.
/// </summary>
/// <param name="InviteCode">The issued invite code.</param>
[EventType]
public record InviteIssued(InviteCode InviteCode);
