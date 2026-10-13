// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace ProjectionStyles.Specs.for_ReducedOrder;

public class when_the_product_is_renamed_before_adding_a_line : given.an_order<ReducedOrder>
{
    async Task Establish()
    {
        await _scenario.Given.ForEventSource(_productId).Events(new ProductRegistered("Trail mug"), new ProductRenamed("Camp mug"));
        await _scenario.Given.ForEventSource(_orderId).Events(new OrderPlaced("Sample customer"), new ItemAddedToOrder(_productId, 2));
    }

    [Fact] void should_not_join_the_product_name() => _order.Lines.Single().ProductName.ShouldBeNull();
    [Fact] void should_still_have_one_line() => _order.ItemCount.ShouldEqual(1);
}
