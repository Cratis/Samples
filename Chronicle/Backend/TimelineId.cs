// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;

namespace Chronicle.Backend;

/// <summary>
/// Represents the unique identifier of a timeline event source.
/// </summary>
/// <param name="Value">The underlying identifier.</param>
public record TimelineId(Guid Value) : EventSourceId<Guid>(Value)
{
    /// <summary>
    /// Gets the identifier used when no timeline has been selected.
    /// </summary>
    public static readonly TimelineId NotSet = new(Guid.Empty);

    /// <summary>
    /// Converts a <see cref="Guid"/> to a <see cref="TimelineId"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The strongly typed timeline identifier.</returns>
    public static implicit operator TimelineId(Guid value) => new(value);

    /// <summary>
    /// Creates a new timeline identifier.
    /// </summary>
    /// <returns>A new <see cref="TimelineId"/>.</returns>
    public static TimelineId New() => new(Guid.NewGuid());
}
