// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Library.Books;

public record LendCount(int Value) : ConceptAs<int>(Value)
{
    public static implicit operator LendCount(int value) => new(value);
}


