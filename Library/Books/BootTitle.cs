// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Library.Books;

public record BootTitle(string Value) : ConceptAs<string>(Value)
{
    public static implicit operator BootTitle(string value) => new(value);
}
