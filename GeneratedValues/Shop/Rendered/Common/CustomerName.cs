// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Concepts;

namespace Shop.Common;

/// <summary>
/// Represents customer name.
/// </summary>
public record CustomerName(string Value) : global::Cratis.Concepts.ConceptAs<string>(Value)
{
    public static readonly CustomerName NotSet = new(string.Empty);

    public static implicit operator string(CustomerName value) => value.Value;
    public static implicit operator CustomerName(string value) => new(value);
}
