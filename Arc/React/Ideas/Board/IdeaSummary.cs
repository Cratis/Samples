// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Arc.React.Ideas.Board;

using Cratis.Arc.Validation;
using Cratis.Concepts;
using FluentValidation;

/// <summary>
/// Represents the useful detail that explains an idea.
/// </summary>
/// <param name="Value">The summary text.</param>
public record IdeaSummary(string Value) : ConceptAs<string>(Value)
{
    /// <summary>
    /// Represents a summary that has not been set.
    /// </summary>
    public static readonly IdeaSummary NotSet = new(string.Empty);

    /// <summary>
    /// Converts a string to an <see cref="IdeaSummary"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    public static implicit operator IdeaSummary(string value) => new(value);
}

/// <summary>
/// Validates the invariant carried by every <see cref="IdeaSummary"/>.
/// </summary>
public class IdeaSummaryValidator : ConceptValidator<IdeaSummary>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdeaSummaryValidator"/> class.
    /// </summary>
    public IdeaSummaryValidator() => RuleFor(_ => _.Value).NotEmpty().MaximumLength(240);
}
