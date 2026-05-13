// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Library;

public record MemberId(Guid Value) : ConceptAs<Guid>(Value)
{
    public static implicit operator MemberId(Guid id) => new(id);

    public static implicit operator Guid(MemberId id) => id.Value;
    public static MemberId New() => new(Guid.NewGuid());
}
