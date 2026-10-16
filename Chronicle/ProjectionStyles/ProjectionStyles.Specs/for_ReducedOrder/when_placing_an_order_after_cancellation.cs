// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace ProjectionStyles.Specs.for_ReducedOrder;

public class when_placing_an_order_after_cancellation : given.an_order<ReducedOrder>
{
    async Task Because() => await _scenario.Given.ForEventSource(_orderId).Events(
        new OrderPlaced("Original customer"),
        new ItemAddedToOrder(_productId, 2),
        new OrderCancelled(),
        new OrderPlaced("New customer"));

    [Fact] void should_create_the_new_order() => _order.CustomerName.ShouldEqual(new CustomerName("New customer"));
    [Fact] void should_not_keep_the_cancelled_orders_lines() => _order.Lines.ShouldBeEmpty();
    [Fact] void should_reset_the_line_count() => _order.ItemCount.ShouldEqual(0);
}
