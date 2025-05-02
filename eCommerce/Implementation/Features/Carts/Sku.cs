// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace eCommerce.Carts;

public record Sku(string Value) : ConceptAs<string>(Value)
{
    public static implicit operator Sku(string value) => new(value);
    public static implicit operator string(Sku price) => price.Value;
}
