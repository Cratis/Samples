// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Library.Common;

public record Address(string Value) : ConceptAs<string>(Value)
{
    public static implicit operator Address(string value) => new(value);
}
