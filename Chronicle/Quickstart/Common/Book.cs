// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Quickstart.Common;

#region Snippet:Quickstart-Book
public record Book(Guid Id, string Title, string Author, string ISBN)
#endregion Snippet:Quickstart-Book
{
    public override string ToString() => Title;
}
