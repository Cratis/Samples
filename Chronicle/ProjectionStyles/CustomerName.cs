// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Compliance.GDPR;
using Cratis.Concepts;

namespace ProjectionStyles;

/// <summary>
/// Represents a customer's display name, classified as personal data.
/// </summary>
/// <param name="Value">The display name.</param>
[PII]
public record CustomerName(string Value) : ConceptAs<string>(Value)
{
    /// <summary>
    /// Converts a display name to the domain concept.
    /// </summary>
    /// <param name="value">The display name.</param>
    /// <returns>The customer name.</returns>
    public static implicit operator CustomerName(string value) => new(value);
}
