// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Commands.ModelBound;

namespace Shop.Carts.Starting;

/// <summary>
/// Before the migration: the handler creates its identity inline, so a spec cannot choose it.
/// </summary>
/// <remarks>
/// Arc reports ARC0028 on the call to <see cref="CartId.New"/> and offers "Move to a [GeneratedValue] parameter".
/// <see cref="StartCart"/> is the result of that migration.
/// </remarks>
[Command]
public record StartCartWithInlineGeneration
{
    /// <summary>
    /// Handles the command.
    /// </summary>
    /// <returns>The identity of the new cart.</returns>
    public CartId Handle() => CartId.New();
}
