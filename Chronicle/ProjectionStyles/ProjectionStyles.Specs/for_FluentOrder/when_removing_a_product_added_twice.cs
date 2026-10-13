// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace ProjectionStyles.Specs.for_FluentOrder;

public class when_removing_a_product_added_twice : given.an_order<FluentOrder>
{
    async Task Establish() => await _scenario.Given.ForEventSource(_orderId).Events(
        new OrderPlaced("Sample customer"), new ItemAddedToOrder(_productId, 2), new ItemAddedToOrder(_productId, 5), new ItemRemovedFromOrder(_productId));

    [Fact] void should_have_no_lines() => _order.Lines.ShouldBeEmpty();
    [Fact] void should_still_count_one_unbalanced_add_event() => _order.ItemCount.ShouldEqual(1);
}
