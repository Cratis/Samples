// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Library.Lenders;

public record LenderId(Guid Value) : ConceptAs<Guid>(Value)
{
    public static implicit operator LenderId(Guid value) => new(value);
    public static implicit operator EventSourceId(LenderId value) => value.Value;

    public static LenderId New() => new(Guid.NewGuid());
}
