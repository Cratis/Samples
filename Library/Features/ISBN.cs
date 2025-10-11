// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Library;

public record ISBN(string Value) : EventSourceId(Value)
{
    public static implicit operator ISBN(string title) => new(title);

    public static implicit operator string(ISBN title) => title.Value;
}
