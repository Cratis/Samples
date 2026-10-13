// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Concepts;

namespace ProjectionStyles;

/// <summary>
/// Represents a product's display name.
/// </summary>
/// <param name="Value">The display name.</param>
public record ProductName(string Value) : ConceptAs<string>(Value)
{
    /// <summary>
    /// Converts a display name to the domain concept.
    /// </summary>
    /// <param name="value">The display name.</param>
    /// <returns>The product name.</returns>
    public static implicit operator ProductName(string value) => new(value);
}
