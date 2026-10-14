// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Validation;
using Cratis.Chronicle.Events;
using Shop.Orders.Placing;

namespace Shop.Specs.Orders.Placing.for_PlaceOrder.when_placing_an_order;

public class with_an_occupied_order_id : Specification
{
    readonly CommandScenario<PlaceOrder> _scenario = new CommandScenario<PlaceOrder>().UseDecisionReads();
    readonly CustomerId _customerId = new("cus_01m4t7txm0hrn5rwemy2dkwtgq");
    readonly OrderId _orderId = new("order_01m4t7txm0fgx9p7tehmp6mprg");
    CommandResult _result = null!;
    EventSequenceNumber _tailBefore;
    EventSequenceNumber _tailAfter;

    async Task Establish()
    {
        _scenario.PinGeneratedValue("orderId", _orderId);
        _scenario.Given.ForEventSource(_orderId).Events(new OrderPlaced(_customerId));
        _tailBefore = await _scenario.EventLog.GetTailSequenceNumber();
    }

    async Task Because()
    {
        _result = await _scenario.Execute(new PlaceOrder(_customerId));
        _tailAfter = await _scenario.EventLog.GetTailSequenceNumber();
    }

    void Destroy() => _scenario.Dispose();

    [Fact] void should_reject_the_command() => _result.IsSuccess.ShouldBeFalse();
    [Fact] void should_report_a_concurrency_violation() => _result.ValidationResults.Single().Reason.ShouldEqual(ValidationResultReason.ConcurrencyViolation);
    [Fact] void should_name_the_collision() => _result.ValidationResults.Single().ReasonDetail.ShouldEqual("GeneratedIdentifierCollision");
    [Fact] void should_append_nothing() => _tailAfter.ShouldEqual(_tailBefore);
}
