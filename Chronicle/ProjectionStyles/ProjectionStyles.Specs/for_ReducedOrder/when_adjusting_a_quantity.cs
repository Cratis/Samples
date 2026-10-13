// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace ProjectionStyles.Specs.for_ReducedOrder;

public class when_adjusting_a_quantity : given.an_order<ReducedOrder>
{
    async Task Establish() => await _scenario.Given.ForEventSource(_orderId).Events(
        new OrderPlaced("Sample customer"), new ItemAddedToOrder(_productId, 2), new ItemAddedToOrder(_otherProductId, 7), new ItemQuantityAdjusted(_productId, 5));

    [Fact] void should_adjust_the_matching_line() => _order.Lines.Single(line => line.ProductId == _productId).Quantity.ShouldEqual(5);
    [Fact] void should_preserve_the_other_line() => _order.Lines.Single(line => line.ProductId == _otherProductId).Quantity.ShouldEqual(7);
    [Fact] void should_preserve_the_derived_count() => _order.ItemCount.ShouldEqual(2);
}
