// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;
using Cratis.Concepts;

namespace Shop.Common;

/// <summary>
/// Represents customer id.
/// </summary>
public record CustomerId(string Value) : global::Cratis.Chronicle.Events.EventSourceId<string>(Value), global::Cratis.Concepts.IGeneratable<CustomerId>
{
    public static readonly CustomerId NotSet = new(string.Empty);

    public static CustomerId New() => new("cus_" + global::Cratis.Concepts.GenerateValue.Ulid().ToLowerInvariant());

    public static implicit operator CustomerId(string value) => new(value);
}
