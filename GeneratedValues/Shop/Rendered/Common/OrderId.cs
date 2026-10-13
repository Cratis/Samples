// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;
using Cratis.Concepts;

namespace Shop.Common;

/// <summary>
/// Represents order id.
/// </summary>
public record OrderId(string Value) : global::Cratis.Chronicle.Events.EventSourceId<string>(Value), global::Cratis.Concepts.IGeneratable<OrderId>
{
    public static readonly OrderId NotSet = new(string.Empty);

    public static OrderId New() => new(global::Cratis.Concepts.GenerateValue.TypeId("order"));

    public static implicit operator OrderId(string value) => new(value);
}
