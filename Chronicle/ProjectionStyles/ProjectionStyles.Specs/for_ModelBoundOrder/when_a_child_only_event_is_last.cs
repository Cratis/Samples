// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace ProjectionStyles.Specs.for_ModelBoundOrder;

public class when_a_child_only_event_is_last : given.an_order<ModelBoundOrder>
{
    DateTimeOffset _addedAt;

    async Task Establish()
    {
        await _scenario.Given.ForEventSource(_orderId).Events(new OrderPlaced("Sample customer"), new ItemAddedToOrder(_productId, 2));
        _addedAt = _order.LastUpdated;
        await _scenario.Given.ForEventSource(_orderId).Events(new ItemQuantityAdjusted(_productId, 5));
    }

    /// <summary>
    /// Synthetic projection contexts advance by one tick per seeded event at the pinned version.
    /// </summary>
    [Fact] void should_update_the_parent_timestamp_from_the_child_event() => _order.LastUpdated.ShouldEqual(_addedAt.AddTicks(1));
    [Fact] void should_preserve_the_line_count() => _order.ItemCount.ShouldEqual(1);
    [Fact] void should_preserve_the_customer() => _order.CustomerName.ShouldEqual(new CustomerName("Sample customer"));
}
