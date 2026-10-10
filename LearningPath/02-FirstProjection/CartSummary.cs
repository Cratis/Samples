// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Projections.ModelBound;

namespace LearningPath.FirstProjection;

/// <summary>
/// Represents the current state of a shopping cart, projected from its events.
/// </summary>
/// <remarks>
/// The attributes are the projection: they tell Chronicle which event changes which property.
/// Chronicle keys each instance by the event source id, the cart's <see cref="CartId"/>.
/// </remarks>
/// <param name="Id">The cart's identity.</param>
/// <param name="ItemCount">The number of items currently in the cart.</param>
/// <param name="CheckedOut">Whether the cart has been checked out.</param>
public record CartSummary(
    CartId Id,

    [AddFrom<ItemAddedToCart>(nameof(ItemAddedToCart.Quantity))]
    [SubtractFrom<ItemRemovedFromCart>(nameof(ItemRemovedFromCart.Quantity))]
    int ItemCount,

    [SetValue<CartCheckedOut>(true)]
    bool CheckedOut);
