// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Concepts;

namespace Shop.Common;

/// <summary>
/// Represents invite code.
/// </summary>
public record InviteCode(string Value) : global::Cratis.Concepts.ConceptAs<string>(Value), global::Cratis.Concepts.IGeneratable<InviteCode>
{
    public static readonly InviteCode NotSet = new(string.Empty);

    public static InviteCode New() => new(global::Cratis.Concepts.GenerateValue.NanoId(10, "23456789ABCDEFGHJKLMNPQRSTUVWXYZ"));

    public static implicit operator string(InviteCode value) => value.Value;
    public static implicit operator InviteCode(string value) => new(value);
}
