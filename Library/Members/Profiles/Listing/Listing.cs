// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.MongoDB;
using Members.Profiles.Registration;
using MongoDB.Driver;

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
    /// <summary>
    /// Observes all member profiles, pushing updates when the collection changes.
    /// </summary>
    /// <param name="collection">The MongoDB collection to observe.</param>
    /// <returns>A subject that emits the full member profile list on each change.</returns>
    public static ISubject<IEnumerable<MemberProfile>> ObserveAll(IMongoCollection<MemberProfile> collection) =>
        collection.Observe();
}

/// <summary>
/// Defines the projection from <see cref="MemberRegistered"/> to <see cref="MemberProfile"/>.
/// </summary>
public class MemberProfileProjection : IProjectionFor<MemberProfile>
{
    /// <inheritdoc/>
    public void Define(IProjectionBuilderFor<MemberProfile> builder) => builder
        .AutoMap()
        .From<MemberRegistered>();
}
