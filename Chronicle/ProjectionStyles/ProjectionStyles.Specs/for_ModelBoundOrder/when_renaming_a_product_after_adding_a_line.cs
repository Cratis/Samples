// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace ProjectionStyles.Specs.for_ModelBoundOrder;

public class when_renaming_a_product_after_adding_a_line : given.an_order<ModelBoundOrder>
{
    async Task Establish()
    {
        await _scenario.Given.ForEventSource(_productId).Events(new ProductRegistered("Trail mug"));
        await _scenario.Given.ForEventSource(_orderId).Events(new OrderPlaced("Sample customer"), new ItemAddedToOrder(_productId, 2));
        await _scenario.Given.ForEventSource(_productId).Events(new ProductRenamed("Camp mug"));
    }

    [Fact] void should_refresh_the_joined_name() => _order.Lines.Single().ProductName.ShouldEqual(new ProductName("Camp mug"));
    [Fact] void should_preserve_the_quantity() => _order.Lines.Single().Quantity.ShouldEqual(2);
    [Fact] void should_not_count_the_join_event() => _order.ItemCount.ShouldEqual(1);
}
