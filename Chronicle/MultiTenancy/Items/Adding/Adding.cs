// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Commands.ModelBound;
using Cratis.Chronicle.Events;

namespace MultiTenancy.Items.Adding;

/// <summary>
/// Adds an item to the current tenant's checklist.
/// </summary>
/// <param name="ItemId">The checklist item identifier.</param>
/// <param name="Text">The item text.</param>
[Command]
public record AddItem(ItemId ItemId, ItemText Text)
{
    /// <summary>
    /// Produces the fact that the item was added.
    /// </summary>
    /// <returns>The event to append to the tenant-scoped event source.</returns>
    public ItemAdded Handle() => new(Text);
}

/// <summary>
/// Records that an item was added to a tenant's checklist.
/// </summary>
/// <param name="Text">The item text.</param>
[EventType]
public record ItemAdded(ItemText Text);
