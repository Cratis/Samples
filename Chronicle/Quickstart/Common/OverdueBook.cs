// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Quickstart.Common;

public record OverdueBook(Guid Id, string Title, Guid UserId, string User, DateTimeOffset Borrowed)
{
    public override string ToString() => Title;
}
