// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.ModelBinding;
using Cratis.Chronicle.Compliance.GDPR;

namespace Members.Profiles.Registration;

/// <summary>
/// Represents a command to register a new member.
/// </summary>
/// <param name="Name">The full name of the member.</param>
/// <param name="Email">The email address of the member.</param>
[Command]
public record RegisterMember(MemberName Name, string Email)
{
    public (MemberId, MemberRegistered) Handle()
    {
        var memberId = MemberId.New();
        return (memberId, new(Name, Email));
    }
}

/// <summary>
/// Represents an event that occurs when a new member registers.
/// </summary>
/// <param name="Name">The full name of the member.</param>
/// <param name="Email">The email address of the member.</param>
[EventType]
public record MemberRegistered(MemberName Name, [PII("Email address used for member communication")] string Email);

/// <summary>
/// Holds the constraint that member email addresses must be unique.
/// </summary>
public class UniqueMemberEmail : IConstraint
{
    public void Define(IConstraintBuilder builder) => builder
        .Unique(_ => _
            .On<MemberRegistered>(e => e.Email)
            .WithMessage("A member with this email address is already registered"));
}
