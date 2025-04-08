// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Library.Books;

public record InventoryCount(int Value) : ConceptAs<int>(Value)
{
    public static implicit operator InventoryCount(int value) => new(value);
}
