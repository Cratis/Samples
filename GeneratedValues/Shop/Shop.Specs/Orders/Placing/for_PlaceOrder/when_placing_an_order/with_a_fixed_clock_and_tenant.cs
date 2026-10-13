// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Tenancy;
using Shop.Orders.Placing;

namespace Shop.Specs.Orders.Placing.for_PlaceOrder.when_placing_an_order;

public class with_a_fixed_clock_and_tenant : Specification
{
    readonly DateTimeOffset _receivedAt = new(2026, 10, 13, 9, 0, 0, TimeSpan.Zero);
    readonly CustomerId _customerId = new("cus_01m4t7txm0hrn5rwemy2dkwtgq");
    CommandScenario<PlaceOrder> _scenario = null!;
    CommandResult _result = null!;
    OrderId _orderId = null!;

    void Establish() => _scenario = new CommandScenario<PlaceOrder>()
        .Clock(_receivedAt)
        .Tenant(new TenantId("north"))
        .AllowGeneration();

    async Task Because()
    {
        _result = await _scenario.Execute(new PlaceOrder(_customerId));
        _orderId = ((CommandResult<OrderId>)_result).Response!;
    }

    void Destroy() => _scenario.Dispose();

    [Fact] void should_succeed() => _result.ShouldBeSuccessful();
    [Fact] void should_generate_an_order_type_id() => _orderId.Value.StartsWith("order_", StringComparison.Ordinal).ShouldBeTrue();
    [Fact] void should_use_the_received_time_as_the_time_part() => TypeIdTime.Of(_orderId.Value).ShouldEqual(_receivedAt);
    [Fact] void should_capture_the_generated_order_id() => _scenario.GeneratedValues.Last<OrderId>("orderId").ShouldEqual(_orderId);
    [Fact] Task should_place_the_order_on_the_generated_id() => _scenario.ShouldHaveAppendedEvent<PlaceOrder, OrderPlaced>(_orderId, @event => @event.CustomerId == _customerId);
}
