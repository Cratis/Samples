// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Quickstart.Common;

#region Snippet:Quickstart-BorrowedBook
public record BorrowedBook(Guid Id, Guid UserId, string Title, string User, DateTimeOffset Borrowed, DateTimeOffset Returned)
#endregion Snippet:Quickstart-BorrowedBook
{
    public override string ToString() => Title;
}
