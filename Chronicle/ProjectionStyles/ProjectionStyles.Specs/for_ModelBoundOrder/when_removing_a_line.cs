// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace ProjectionStyles.Specs.for_ModelBoundOrder;

public class when_removing_a_line : given.an_order<ModelBoundOrder>
{
    async Task Establish() => await _scenario.Given.ForEventSource(_orderId).Events(
        new OrderPlaced("Sample customer"), new ItemAddedToOrder(_productId, 2), new ItemAddedToOrder(_otherProductId, 7), new ItemRemovedFromOrder(_productId));

    [Fact] void should_remove_only_the_matching_line() => _order.Lines.Select(line => line.ProductId).ShouldContainOnly(_otherProductId);
    [Fact] void should_decrement_the_counter() => _order.ItemCount.ShouldEqual(1);
}
