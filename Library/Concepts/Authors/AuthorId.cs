// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Authors;

public record AuthorId(Guid Value) : ConceptAs<Guid>(Value)
{
    public static implicit operator AuthorId(Guid value) => new(value);
}
