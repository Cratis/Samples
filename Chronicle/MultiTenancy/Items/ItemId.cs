// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle;

namespace MultiTenancy.Items;

/// <summary>
/// Identifies one checklist item event source.
/// </summary>
/// <param name="Value">The underlying identifier.</param>
public record ItemId(Guid Value) : EventSourceId<Guid>(Value)
{
    /// <summary>
    /// Represents an identifier that has not been set.
    /// </summary>
    public static readonly ItemId NotSet = new(Guid.Empty);

    /// <summary>
    /// Converts a <see cref="Guid"/> to an <see cref="ItemId"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    public static implicit operator ItemId(Guid value) => new(value);

    /// <summary>
    /// Creates a new checklist item identifier.
    /// </summary>
    /// <returns>A new checklist item identifier.</returns>
    public static ItemId New() => new(Guid.NewGuid());
}
