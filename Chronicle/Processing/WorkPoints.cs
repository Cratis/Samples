// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Concepts;

namespace Processing;

/// <summary>
/// Represents an amount of work measured in points.
/// </summary>
/// <param name="Value">The underlying number of points.</param>
public record WorkPoints(int Value) : ConceptAs<int>(Value)
{
    /// <summary>
    /// Represents an unset amount of work.
    /// </summary>
    public static readonly WorkPoints NotSet = new(0);

    /// <summary>
    /// Converts an integer to work points.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted work points.</returns>
    public static implicit operator WorkPoints(int value) => new(value);
}
