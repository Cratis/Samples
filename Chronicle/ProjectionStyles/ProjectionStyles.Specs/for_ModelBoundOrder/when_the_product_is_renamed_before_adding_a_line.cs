// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace ProjectionStyles.Specs.for_ModelBoundOrder;

public class when_the_product_is_renamed_before_adding_a_line : given.an_order<ModelBoundOrder>
{
    async Task Establish()
    {
        await _scenario.Given.ForEventSource(_productId).Events(new ProductRegistered("Trail mug"), new ProductRenamed("Camp mug"));
        await _scenario.Given.ForEventSource(_orderId).Events(new OrderPlaced("Sample customer"), new ItemAddedToOrder(_productId, 2));
    }

    [Fact] void should_join_the_latest_name() => _order.Lines.Single().ProductName.ShouldEqual(new ProductName("Camp mug"));
    [Fact] void should_only_create_the_order() => _scenario.Instances.Keys.ShouldContainOnly((Cratis.Chronicle.Events.EventSourceId)_orderId);
}
