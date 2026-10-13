// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Shop.Orders.Placing;

namespace Shop.Specs.Orders.Placing.for_PlaceOrder.when_placing_an_order;

public class without_a_pin : Specification
{
    readonly CommandScenario<PlaceOrder> _scenario = new();
    CommandResult _result = null!;

    async Task Because() => _result = await _scenario.Execute(new PlaceOrder(new CustomerId("cus_01m4t7txm0hrn5rwemy2dkwtgq")));
    void Destroy() => _scenario.Dispose();

    [Fact] void should_fail_the_command() => _result.ShouldNotBeSuccessful();
    [Fact] void should_report_an_exception_not_a_validation_failure() => _result.ShouldHaveExceptions();
    [Fact] void should_explain_how_to_pin_it() => _result.ExceptionMessages.Any(message => message.Contains("PinGeneratedValue(\"orderId\",", StringComparison.Ordinal)).ShouldBeTrue();
    [Fact] void should_not_capture_a_value() => _scenario.GeneratedValues.All<OrderId>("orderId").ShouldBeEmpty();
}
