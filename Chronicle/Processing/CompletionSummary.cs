// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Concepts;

namespace Processing;

/// <summary>
/// Represents a summary of completed work against its plan.
/// </summary>
/// <param name="Value">The underlying summary.</param>
public record CompletionSummary(string Value) : ConceptAs<string>(Value)
{
    /// <summary>
    /// Represents an unset completion summary.
    /// </summary>
    public static readonly CompletionSummary NotSet = new(string.Empty);

    /// <summary>
    /// Converts a string to a completion summary.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted completion summary.</returns>
    public static implicit operator CompletionSummary(string value) => new(value);
}
