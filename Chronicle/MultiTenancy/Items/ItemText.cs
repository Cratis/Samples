// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Concepts;

namespace MultiTenancy.Items;

/// <summary>
/// Represents the text of a checklist item.
/// </summary>
/// <param name="Value">The text value.</param>
public record ItemText(string Value) : ConceptAs<string>(Value)
{
    /// <summary>
    /// Represents text that has not been set.
    /// </summary>
    public static readonly ItemText NotSet = new(string.Empty);

    /// <summary>
    /// Converts a string to checklist item text.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    public static implicit operator ItemText(string value) => new(value);
}
