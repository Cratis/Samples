// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace ProjectionStyles.Specs.for_ModelBoundOrder;

public class when_placing_an_order : given.an_order<ModelBoundOrder>
{
    async Task Establish() => await _scenario.Given.ForEventSource(_orderId).Events(new OrderPlaced("Sample customer"));

    [Fact] void should_use_the_order_stream_identity() => _order.Id.ShouldEqual(_orderId);
    [Fact] void should_have_the_customer_name() => _order.CustomerName.ShouldEqual(new CustomerName("Sample customer"));
    [Fact] void should_not_be_shipped() => _order.IsShipped.ShouldBeFalse();
    [Fact] void should_start_at_zero() => _order.ItemCount.ShouldEqual(0);
    [Fact] void should_have_no_lines() => _order.Lines.ShouldBeEmpty();
}
