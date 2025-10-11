// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using eCommerce.Carts.AddItem;
using MongoDB.Driver;

namespace eCommerce.Carts.Contents;

public record CartItem(Sku Sku, Price Price);

[ReadModel]
public record Cart(CartId Id, IEnumerable<CartItem> Items, Price TotalPrice)
{
    public static ISubject<Cart> CartById(CartId cartId, IMongoCollection<Cart> collection) =>
        collection.ObserveById(cartId);
}

public class CartProjection : IProjectionFor<Cart>
{
    public void Define(IProjectionBuilderFor<Cart> builder) => builder
        .AutoMap()
        .From<ItemAddedToCart>(e => e.Add(m => m.TotalPrice).With(e => e.Price))
        .Children(c => c.Items, childrenBuilder => childrenBuilder
            .From<ItemAddedToCart>());
}
