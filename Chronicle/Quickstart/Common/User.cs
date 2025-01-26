// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Quickstart.Common;

#region Snippet:Quickstart-User
public record User(Guid Id, string Name, string Email)
#endregion Snippet:Quickstart-User
{
    public override string ToString() => Name;
}
