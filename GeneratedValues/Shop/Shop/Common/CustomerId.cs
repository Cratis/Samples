// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;
using Cratis.Concepts;

namespace Shop.Common;

/// <summary>
/// Represents the identity of a customer: "cus_" followed by a lowercase ULID.
/// </summary>
/// <param name="Value">The underlying text value.</param>
public record CustomerId(string Value) : EventSourceId<string>(Value), IGeneratable<CustomerId>
{
    /// <summary>
    /// Converts text to a <see cref="CustomerId"/>.
    /// </summary>
    /// <param name="value">The text to convert.</param>
    public static implicit operator CustomerId(string value) => new(value);

    /// <summary>
    /// Generates a new customer identity.
    /// </summary>
    /// <returns>A new <see cref="CustomerId"/>.</returns>
    public static CustomerId New() => new($"cus_{GenerateValue.Ulid().ToLowerInvariant()}");
}
