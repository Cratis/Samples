// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;

namespace LearningPath.EventVersioning.Carts;

/// <summary>
/// Represents the original addition event, before the shop recorded unit prices.
/// </summary>
/// <param name="Product">The product that was added.</param>
/// <param name="Quantity">How many were added.</param>
[EventTypeGenerationFor<ItemAddedToCart>(1)]
public record ItemAddedToCartV1(ProductName Product, Quantity Quantity);
