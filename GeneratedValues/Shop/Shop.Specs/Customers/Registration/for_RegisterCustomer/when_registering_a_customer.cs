// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Shop.Customers.Registration;

namespace Shop.Specs.Customers.Registration.for_RegisterCustomer;

public class when_registering_a_customer : Specification
{
    readonly CommandScenario<RegisterCustomer> _scenario = new();
    readonly CustomerId _customerId = new("cus_01m4t7txm0hrn5rwemy2dkwtgq");
    CommandResult _result = null!;

    void Establish() => _scenario.PinGeneratedValue("customerId", _customerId);
    async Task Because() => _result = await _scenario.Execute(new RegisterCustomer("Ada Lovelace"));
    void Destroy() => _scenario.Dispose();

    [Fact] void should_succeed() => _result.ShouldBeSuccessful();
    [Fact] void should_return_the_pinned_customer_id() => ((CommandResult<CustomerId>)_result).Response.ShouldEqual(_customerId);
    [Fact] void should_capture_the_pinned_customer_id() => _scenario.GeneratedValues.Last<CustomerId>("customerId").ShouldEqual(_customerId);
    [Fact] Task should_register_the_customer_on_the_pinned_id() => _scenario.ShouldHaveAppendedEvent<RegisterCustomer, CustomerRegistered>(_customerId, @event => @event.Name == new CustomerName("Ada Lovelace"));
}
