// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Members.Profiles.Registration;

namespace Members.Profiles.Listing;

/// <summary>
/// Represents a member profile read model.
/// </summary>
/// <param name="Id">The unique identifier of the member.</param>
/// <param name="Name">The full name of the member.</param>
/// <param name="Email">The email address of the member.</param>
[ReadModel]
public record MemberProfile(MemberId Id, MemberName Name, string Email)
{
    public static IObservable<IEnumerable<MemberProfile>> AllMembers(IMaterializedReadModels readModels) =>
        readModels.ObserveInstances<MemberProfile>();
}

public class MemberProfileProjection : IProjectionFor<MemberProfile>
{
    public void Define(IProjectionBuilderFor<MemberProfile> builder) => builder
        .AutoMap()
        .From<MemberRegistered>();
}
