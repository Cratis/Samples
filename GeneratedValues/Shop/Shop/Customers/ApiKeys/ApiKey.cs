// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Chronicle.Commands;
using Cratis.Concepts;

namespace Shop.Customers.ApiKeys;

/// <summary>
/// Represents an API key: 32 random bytes as URL-safe base64 without padding (256 random bits).
/// </summary>
/// <remarks>
/// <see cref="NotAuditedAttribute"/> travels with the type: wherever a command carries an <see cref="ApiKey"/>,
/// its value is left out of the causation chain of the events that command appends. It does not keep the value
/// out of event payloads, command responses or logs; keep it out of those yourself.
/// </remarks>
/// <param name="Value">The underlying text value.</param>
[NotAudited]
public record ApiKey(string Value) : ConceptAs<string>(Value), IGeneratable<ApiKey>
{
    /// <summary>
    /// Generates a new API key.
    /// </summary>
    /// <returns>A new <see cref="ApiKey"/>.</returns>
    public static ApiKey New() => new(GenerateValue.Bytes(32, ValueEncoding.Base64Url));
}
