// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Concepts;

namespace Chronicle.Backend;

/// <summary>
/// Represents the text captured in a timeline entry.
/// </summary>
/// <param name="Value">The entry text.</param>
public record TimelineEntryText(string Value) : ConceptAs<string>(Value)
{
    /// <summary>
    /// Converts text to a timeline entry value.
    /// </summary>
    /// <param name="value">The text to convert.</param>
    public static implicit operator TimelineEntryText(string value) => new(value);

    /// <summary>
    /// Converts a timeline entry value to text.
    /// </summary>
    /// <param name="value">The timeline entry value.</param>
    public static implicit operator string(TimelineEntryText value) => value.Value;
}
