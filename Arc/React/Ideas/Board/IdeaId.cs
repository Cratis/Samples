// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Arc.React.Ideas.Board;

using Cratis.Concepts;

/// <summary>
/// Represents the identifier of an idea stored as current state.
/// </summary>
/// <param name="Value">The underlying identifier.</param>
public record IdeaId(Guid Value) : ConceptAs<Guid>(Value)
{
    /// <summary>
    /// Represents an identifier that has not been set.
    /// </summary>
    public static readonly IdeaId NotSet = new(Guid.Empty);

    /// <summary>
    /// Converts a <see cref="Guid"/> to an <see cref="IdeaId"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    public static implicit operator IdeaId(Guid value) => new(value);

    /// <summary>
    /// Creates a new idea identifier.
    /// </summary>
    /// <returns>A new idea identifier.</returns>
    public static IdeaId New() => new(Guid.NewGuid());
}
