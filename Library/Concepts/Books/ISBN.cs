// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Books;

public record ISBN(string Value) : ConceptAs<string>(Value)
{
    public static implicit operator ISBN(string value) => new(value);
}
