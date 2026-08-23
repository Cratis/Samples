// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Validation;
using Cratis.Concepts;
using FluentValidation;

namespace Arc.React.Ideas.Board;

/// <summary>
/// Represents the concise title of an idea.
/// </summary>
/// <param name="Value">The title text.</param>
public record IdeaTitle(string Value) : ConceptAs<string>(Value)
{
    /// <summary>
    /// Represents a title that has not been set.
    /// </summary>
    public static readonly IdeaTitle NotSet = new(string.Empty);

    /// <summary>
    /// Converts a string to an <see cref="IdeaTitle"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    public static implicit operator IdeaTitle(string value) => new(value);
}

/// <summary>
/// Validates the invariant carried by every <see cref="IdeaTitle"/>.
/// </summary>
public class IdeaTitleValidator : ConceptValidator<IdeaTitle>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdeaTitleValidator"/> class.
    /// </summary>
    public IdeaTitleValidator() => RuleFor(_ => _.Value).NotEmpty().MaximumLength(72);
}
