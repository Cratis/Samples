// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Shop.Orders.Placing;

namespace Shop.Specs.Orders.Placing.for_PlaceOrder.when_placing_an_order;

public class with_a_pinned_order_id : Specification
{
    readonly CommandScenario<PlaceOrder> _scenario = new();
    readonly CustomerId _customerId = new("cus_01m4t7txm0hrn5rwemy2dkwtgq");
    readonly OrderId _orderId = new("order_01m4t7txm0fgx9p7tehmp6mprg");
    CommandResult _result = null!;

    void Establish() => _scenario.PinGeneratedValue("orderId", _orderId);
    async Task Because() => _result = await _scenario.Execute(new PlaceOrder(_customerId));
    void Destroy() => _scenario.Dispose();

    [Fact] void should_succeed() => _result.ShouldBeSuccessful();
    [Fact] void should_return_the_pinned_order_id() => ((CommandResult<OrderId>)_result).Response.ShouldEqual(_orderId);
    [Fact] Task should_place_the_order_on_the_pinned_id() => _scenario.ShouldHaveAppendedEvent<PlaceOrder, OrderPlaced>(_orderId, @event => @event.CustomerId == _customerId);
}
