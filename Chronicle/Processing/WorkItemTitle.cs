// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Concepts;

namespace Processing;

/// <summary>
/// Represents the title of a work item.
/// </summary>
/// <param name="Value">The underlying title.</param>
public record WorkItemTitle(string Value) : ConceptAs<string>(Value)
{
    /// <summary>
    /// Represents an unset work item title.
    /// </summary>
    public static readonly WorkItemTitle NotSet = new(string.Empty);

    /// <summary>
    /// Converts a string to a work item title.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted work item title.</returns>
    public static implicit operator WorkItemTitle(string value) => new(value);
}
