// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Authors;

public record AuthorName(string Value) : ConceptAs<string>(Value)
{
    public static implicit operator AuthorName(string value) => new(value);
}
