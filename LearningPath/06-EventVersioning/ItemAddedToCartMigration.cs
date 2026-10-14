// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events.Migrations;

namespace LearningPath.EventVersioning.Carts;

/// <summary>
/// Describes how the same addition fact is represented before and after unit prices were recorded.
/// </summary>
public class ItemAddedToCartMigration : EventTypeMigration<ItemAddedToCart, ItemAddedToCartV1>
{
    /// <inheritdoc/>
    public override void Upcast(IEventMigrationBuilder<ItemAddedToCart, ItemAddedToCartV1> builder) =>
        builder.Properties(properties => properties.DefaultValue(@event => @event.UnitPrice, Price.NotRecorded));

    /// <inheritdoc/>
    public override void Downcast(IEventMigrationBuilder<ItemAddedToCartV1, ItemAddedToCart> builder)
    {
        // Chronicle walks the target schema: Product and Quantity survive; UnitPrice is not in generation 1.
        // No property operations are needed to drop it.
    }
}
