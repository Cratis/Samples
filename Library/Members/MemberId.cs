// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Members;

public record MemberId(Guid Value) : ConceptAs<Guid>(Value)
{
    /// <summary>
    /// Gets the sentinel value representing an unset <see cref="MemberId"/>.
    /// </summary>
    public static readonly MemberId NotSet = new(Guid.Empty);

    public static implicit operator MemberId(Guid id) => new(id);

    public static implicit operator Guid(MemberId id) => id.Value;

    public static implicit operator EventSourceId(MemberId id) => new(id.Value.ToString());

    /// <summary>
    /// Creates a new <see cref="MemberId"/> with a unique identifier.
    /// </summary>
    /// <returns>A new <see cref="MemberId"/>.</returns>
    public static MemberId New() => new(Guid.NewGuid());
}
