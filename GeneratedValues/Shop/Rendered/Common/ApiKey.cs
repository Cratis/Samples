// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Concepts;

namespace Shop.Common;

/// <summary>
/// Represents api key.
/// </summary>
public record ApiKey(string Value) : global::Cratis.Concepts.ConceptAs<string>(Value), global::Cratis.Concepts.IGeneratable<ApiKey>
{
    public static readonly ApiKey NotSet = new(string.Empty);

    public static ApiKey New() => new(global::Cratis.Concepts.GenerateValue.Bytes(32, global::Cratis.Concepts.ValueEncoding.Base64Url));

    public static implicit operator string(ApiKey value) => value.Value;
    public static implicit operator ApiKey(string value) => new(value);
}
