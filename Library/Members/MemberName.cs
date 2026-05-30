// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Members;

public record MemberName(string Value) : ConceptAs<string>(Value)
{
    public static readonly MemberName Empty = new(string.Empty);

    public static implicit operator MemberName(string value) => new(value);

    public static implicit operator string(MemberName value) => value.Value;
}
