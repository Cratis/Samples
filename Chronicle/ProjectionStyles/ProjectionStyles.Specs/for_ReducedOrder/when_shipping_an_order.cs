// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace ProjectionStyles.Specs.for_ReducedOrder;

public class when_shipping_an_order : given.an_order<ReducedOrder>
{
    async Task Establish() => await _scenario.Given.ForEventSource(_orderId).Events(
        new OrderPlaced("Sample customer"), new ItemAddedToOrder(_productId, 2), new OrderShipped());

    [Fact] void should_mark_the_order_shipped() => _order.IsShipped.ShouldBeTrue();
    [Fact] void should_preserve_the_line() => _order.Lines.Single().ProductId.ShouldEqual(_productId);
}
