// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;

namespace Processing;

/// <summary>
/// Represents the identity of a work item.
/// </summary>
/// <param name="Value">The underlying identifier.</param>
public record WorkItemId(Guid Value) : EventSourceId<Guid>(Value)
{
    /// <summary>
    /// Represents an unset work item identity.
    /// </summary>
    public static readonly WorkItemId NotSet = new(Guid.Empty);

    /// <summary>
    /// Converts a <see cref="Guid"/> to a work item identity.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted work item identity.</returns>
    public static implicit operator WorkItemId(Guid value) => new(value);

    /// <summary>
    /// Creates a new work item identity.
    /// </summary>
    /// <returns>A new work item identity.</returns>
    public static WorkItemId New() => new(Guid.NewGuid());
}
