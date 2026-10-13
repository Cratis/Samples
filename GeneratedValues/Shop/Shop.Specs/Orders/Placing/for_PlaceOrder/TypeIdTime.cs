// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Shop.Specs.Orders.Placing.for_PlaceOrder;

/// <summary>
/// Reads the time part of a TypeID: the 48-bit Unix millisecond timestamp at the start of its UUIDv7 suffix.
/// </summary>
static class TypeIdTime
{
    const string CrockfordLower = "0123456789abcdefghjkmnpqrstvwxyz";

    /// <summary>
    /// Gets the time encoded in a TypeID.
    /// </summary>
    /// <param name="typeId">The TypeID text, such as <c>order_01m4t7txm0fgx9p7tehmp6mprg</c>.</param>
    /// <returns>The encoded time, with millisecond precision.</returns>
    public static DateTimeOffset Of(string typeId)
    {
        var suffix = typeId[(typeId.LastIndexOf('_') + 1)..];
        var uuid = suffix.Aggregate(UInt128.Zero, (value, character) => (value << 5) | (uint)CrockfordLower.IndexOf(character, StringComparison.Ordinal));

        return DateTimeOffset.FromUnixTimeMilliseconds((long)(uuid >> 80));
    }
}
