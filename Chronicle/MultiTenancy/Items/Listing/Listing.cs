// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Queries.ModelBound;
using Cratis.Chronicle;
using Cratis.Chronicle.Projections.ModelBound;
using Cratis.Chronicle.ReadModels;
using MultiTenancy.Items.Adding;

namespace MultiTenancy.Items.Listing;

/// <summary>
/// Represents one tenant-scoped checklist item.
/// </summary>
/// <param name="Id">The checklist item identifier.</param>
/// <param name="Text">The item text.</param>
[ReadModel]
[FromEvent<ItemAdded>]
public record Item(ItemId Id, ItemText Text)
{
    /// <summary>
    /// Gets one checklist item from the current tenant's Chronicle namespace.
    /// </summary>
    /// <param name="readModels">The Chronicle read models.</param>
    /// <param name="id">The checklist item identifier.</param>
    /// <returns>The item when it exists in the current tenant, otherwise <see langword="null"/>.</returns>
    public static async Task<Item?> ItemById(IReadModels readModels, ItemId id) =>
        await readModels.GetInstanceById<Item>((EventSourceId)id);
}
