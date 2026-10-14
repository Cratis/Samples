// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Keys;
using Cratis.Chronicle.Projections;
using Cratis.Chronicle.Projections.ModelBound;

namespace LearningPath.EventVersioning.Carts;

/// <summary>
/// Represents one product's quantity and last recorded unit price in a cart.
/// </summary>
/// <param name="Product">The product identifying this line.</param>
/// <param name="Quantity">The running quantity, including additions and removals.</param>
/// <param name="UnitPrice">The price from the most recent addition, or not recorded for generation 1.</param>
[NoAutoMap]
public record CartLine(
    [Key]
    [SetFrom<ItemAddedToCart>(nameof(ItemAddedToCart.Product))]
    [SetFrom<ItemRemovedFromCart>(nameof(ItemRemovedFromCart.Product))]
    ProductName Product,

    [AddFrom<ItemAddedToCart>(nameof(ItemAddedToCart.Quantity))]
    [SubtractFrom<ItemRemovedFromCart>(nameof(ItemRemovedFromCart.Quantity))]
    int Quantity,

    [SetFrom<ItemAddedToCart>(nameof(ItemAddedToCart.UnitPrice))]
    Price UnitPrice);
