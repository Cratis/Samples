// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace ProjectionStyles.Specs.for_ReducedOrder;

public class when_a_line_event_is_last : given.an_order<ReducedOrder>
{
    DateTimeOffset _addedAt;

    async Task Establish()
    {
        await _scenario.Given.ForEventSource(_orderId).Events(new OrderPlaced("Sample customer"), new ItemAddedToOrder(_productId, 2));
        _addedAt = _order.LastUpdated;
        await _scenario.Given.ForEventSource(_orderId).Events(new ItemQuantityAdjusted(_productId, 5));
    }

    [Fact] void should_advance_the_order_timestamp() => _order.LastUpdated.ShouldBeGreaterThan(_addedAt);
    [Fact] void should_preserve_the_derived_line_count() => _order.ItemCount.ShouldEqual(1);
    [Fact] void should_preserve_the_customer() => _order.CustomerName.ShouldEqual(new CustomerName("Sample customer"));
}
