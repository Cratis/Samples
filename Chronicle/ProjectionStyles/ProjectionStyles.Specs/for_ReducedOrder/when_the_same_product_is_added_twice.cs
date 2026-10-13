// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace ProjectionStyles.Specs.for_ReducedOrder;

public class when_the_same_product_is_added_twice : given.an_order<ReducedOrder>
{
    async Task Establish() => await _scenario.Given.ForEventSource(_orderId).Events(
        new OrderPlaced("Sample customer"), new ItemAddedToOrder(_productId, 2), new ItemAddedToOrder(_productId, 5));

    [Fact] void should_have_one_line() => _order.ItemCount.ShouldEqual(1);
    [Fact] void should_have_one_keyed_line() => _order.Lines.Count().ShouldEqual(1);
    [Fact] void should_replace_the_quantity() => _order.Lines.Single().Quantity.ShouldEqual(5);
}
