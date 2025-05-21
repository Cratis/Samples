// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Library;

public record AuthorName(string Value) : ConceptAs<string>(Value)
{
    public static implicit operator string(AuthorName authorName) => authorName.Value;
    public static implicit operator AuthorName(string value) => new(value);
}
