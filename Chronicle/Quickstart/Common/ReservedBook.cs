// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Quickstart.Common;

public record ReservedBook(Guid Id, Guid UserId, string Title, string User, DateTimeOffset Reserved)
{
    public override string ToString() => Title;
}
