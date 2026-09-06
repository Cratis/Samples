// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;

namespace OperationsDiagnosis;

/// <summary>
/// Represents the identity of an operations probe.
/// </summary>
/// <param name="Value">The underlying identifier.</param>
public record ProbeId(Guid Value) : EventSourceId<Guid>(Value)
{
    /// <summary>
    /// Represents an unset operations probe identity.
    /// </summary>
    public static readonly ProbeId NotSet = new(Guid.Empty);

    /// <summary>
    /// Represents the stable event source used by the failure fixture.
    /// </summary>
    public static readonly ProbeId FailureFixture = new(new Guid(0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0xd1, 0xa6) /* 00000000-0000-0000-0000-00000000d1a6 */);

    /// <summary>
    /// Converts a <see cref="Guid"/> to an operations probe identity.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted operations probe identity.</returns>
    public static implicit operator ProbeId(Guid value) => new(value);

    /// <summary>
    /// Creates a new operations probe identity.
    /// </summary>
    /// <returns>A new operations probe identity.</returns>
    public static ProbeId New() => new(Guid.NewGuid());
}
