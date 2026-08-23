// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Concepts;

namespace OperationsDiagnosis;

/// <summary>
/// Represents the name of an operations probe.
/// </summary>
/// <param name="Value">The underlying name.</param>
public record ProbeName(string Value) : ConceptAs<string>(Value)
{
    /// <summary>
    /// Represents an unset operations probe name.
    /// </summary>
    public static readonly ProbeName NotSet = new(string.Empty);

    /// <summary>
    /// Represents the probe that deliberately fails the observer.
    /// </summary>
    public static readonly ProbeName FailureFixture = new("operations-diagnosis-canary");

    /// <summary>
    /// Converts a string to an operations probe name.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted operations probe name.</returns>
    public static implicit operator ProbeName(string value) => new(value);
}
