// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Compliance.GDPR;

namespace Members;

/// <summary>
/// Represents the full name of a library member.
/// </summary>
/// <param name="Value">The string value of the name.</param>
[PII("Full name of the library member — used for identification and correspondence")]
public record MemberName(string Value) : ConceptAs<string>(Value)
{
    /// <summary>
    /// Gets an empty <see cref="MemberName"/>.
    /// </summary>
    public static readonly MemberName Empty = new(string.Empty);

    /// <summary>
    /// Implicitly converts a <see cref="string"/> to a <see cref="MemberName"/>.
    /// </summary>
    /// <param name="value">The string value.</param>
    public static implicit operator MemberName(string value) => new(value);

    /// <summary>
    /// Implicitly converts a <see cref="MemberName"/> to a <see cref="string"/>.
    /// </summary>
    /// <param name="value">The <see cref="MemberName"/> to convert.</param>
    public static implicit operator string(MemberName value) => value.Value;
}
