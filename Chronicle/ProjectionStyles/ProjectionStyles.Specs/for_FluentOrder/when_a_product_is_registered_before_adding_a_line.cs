// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace ProjectionStyles.Specs.for_FluentOrder;

public class when_a_product_is_registered_before_adding_a_line : given.an_order<FluentOrder>
{
    async Task Establish()
    {
        await _scenario.Given.ForEventSource(_productId).Events(new ProductRegistered("Trail mug"));
        await _scenario.Given.ForEventSource(_orderId).Events(new OrderPlaced("Sample customer"), new ItemAddedToOrder(_productId, 2));
    }

    [Fact] void should_join_the_registered_name() => _order.Lines.Single().ProductName.ShouldEqual(new ProductName("Trail mug"));
}
