// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Library.Common;

public record FirstName(string Value) : ConceptAs<string>(Value)
{
    public static implicit operator FirstName(string value) => new(value);
}
