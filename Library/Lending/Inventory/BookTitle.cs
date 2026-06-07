// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Library.Inventory;

public record BookTitle(string Value) : ConceptAs<string>(Value)
{
    public static implicit operator BookTitle(string title) => new(title);

    public static implicit operator string(BookTitle title) => title.Value;
}
