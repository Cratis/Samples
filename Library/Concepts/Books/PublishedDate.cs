// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Books;

public record PublishedDate(DateTime Value) : ConceptAs<DateTime>(Value)
{
    public static implicit operator PublishedDate(DateTime value) => new(value);
}
