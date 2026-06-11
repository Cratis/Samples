// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Identity;

namespace Members.Identity;

/// <summary>
/// Represents the Members-specific identity details for the current user.
/// </summary>
/// <param name="MemberId">The unique identifier of the member this identity belongs to.</param>
public record MemberIdentityDetails(MemberId MemberId)
{
    /// <summary>
    /// Gets the sentinel value representing an unauthenticated identity.
    /// </summary>
    public static readonly MemberIdentityDetails NotSet = new(MemberId.NotSet);
}

/// <summary>
/// Provides Members-specific identity details to Cratis Arc, enriching the identity response
/// served to frontend clients via the <c>/api/identity</c> endpoint.
/// </summary>
public class MemberIdentityDetailsProvider : IProvideIdentityDetails<MemberIdentityDetails>
{
    /// <inheritdoc/>
    public Task<IdentityDetails> Provide(IdentityProviderContext context)
    {
        var memberId = Guid.TryParse(context.Id.Value, out var parsedId)
            ? new MemberId(parsedId)
            : MemberId.NotSet;
        var details = new MemberIdentityDetails(memberId);

        return Task.FromResult(new IdentityDetails(memberId != MemberId.NotSet, details));
    }
}
