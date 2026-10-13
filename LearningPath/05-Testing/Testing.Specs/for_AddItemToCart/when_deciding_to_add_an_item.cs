// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace LearningPath.Testing.Carts.for_AddItemToCart;

public class when_deciding_to_add_an_item : Specification
{
    ItemAddedToCart _event;

    void Because() => _event = new AddItemToCart(CartId.New(), "Coffee beans", 2).Handle();

    [Fact] void should_record_the_product() => _event.Product.Value.ShouldEqual("Coffee beans");
    [Fact] void should_record_the_quantity() => _event.Quantity.Value.ShouldEqual(2);
}
