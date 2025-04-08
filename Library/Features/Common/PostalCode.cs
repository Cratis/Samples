// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Library.Common;

public record PostalCode(string Value) : ConceptAs<string>(Value)
{
    public static implicit operator PostalCode(string value) => new(value);
}
