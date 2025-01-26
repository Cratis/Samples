// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Books;

public record BookId(Guid Value) : ConceptAs<Guid>(Value)
{
    public static implicit operator BookId(Guid value) => new(value);
}
