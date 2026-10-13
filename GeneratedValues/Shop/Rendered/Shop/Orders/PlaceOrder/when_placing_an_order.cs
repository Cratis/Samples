#if DEBUG
// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Chronicle.Testing.Commands;
using Cratis.Arc.Commands;
using Cratis.Arc.Testing.Commands;
using Cratis.Chronicle.Events;
using Cratis.Specifications;
using Shop.Common;
using Xunit;

namespace Shop.Shop.Orders.PlaceOrder.when_placing_an_order;

public class when_placing_an_order : global::Cratis.Specifications.Specification, global::System.IDisposable
{
    readonly global::Cratis.Arc.Testing.Commands.CommandScenario<global::Shop.Shop.Orders.PlaceOrder.PlaceOrder> _scenario = new();
    global::Cratis.Arc.Commands.CommandResult _result = null!;

    void Establish()
    {
        _scenario.PinGeneratedValue("orderId", new global::Shop.Common.OrderId("order_01m4t7txm0fgx9p7tehmp6mprg"));
    }

    async global::System.Threading.Tasks.Task Because() => _result = await _scenario.Execute(new global::Shop.Shop.Orders.PlaceOrder.PlaceOrder(new global::Shop.Common.CustomerId("cus_01m4t7txm0hrn5rwemy2dkwtgq")));

    [global::Xunit.FactAttribute] void should_succeed() => _result.ShouldBeSuccessful();
    [global::Xunit.FactAttribute] async global::System.Threading.Tasks.Task should_have_appended_order_placed() => await _scenario.ShouldHaveAppendedEvent<global::Shop.Shop.Orders.PlaceOrder.PlaceOrder, global::Shop.Shop.Orders.PlaceOrder.OrderPlaced>(new global::Shop.Common.OrderId("order_01m4t7txm0fgx9p7tehmp6mprg"), @event => @event.CustomerId == new global::Shop.Common.CustomerId("cus_01m4t7txm0hrn5rwemy2dkwtgq"));
    [global::Xunit.FactAttribute] void should_return_the_expected_response() => ((global::Cratis.Arc.Commands.CommandResult<global::Shop.Common.OrderId>)_result).Response.ShouldEqual(new global::Shop.Common.OrderId("order_01m4t7txm0fgx9p7tehmp6mprg"));

    public void Dispose() => _scenario.Dispose();
}

#endif
