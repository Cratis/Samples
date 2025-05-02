// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using eCommerce.Carts.AddItem;

namespace eCommerce.Carts;

public class Cart : AggregateRoot
{
    public Task AddItem(Sku sku, Price price)
    {
        if (IsNew)
        {
            Apply(new CartCreated());
        }

        Apply(new ItemAddedToCart(sku, price));
        return Task.CompletedTask;
    }
}
