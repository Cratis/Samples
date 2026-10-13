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

namespace Shop.Shop.Customers.RegisterCustomer.when_registering_acustomer;

public class when_registering_acustomer : global::Cratis.Specifications.Specification, global::System.IDisposable
{
    readonly global::Cratis.Arc.Testing.Commands.CommandScenario<global::Shop.Shop.Customers.RegisterCustomer.RegisterCustomer> _scenario = new();
    global::Cratis.Arc.Commands.CommandResult _result = null!;

    void Establish()
    {
        _scenario.PinGeneratedValue("customerId", new global::Shop.Common.CustomerId("cus_01m4t7txm0hrn5rwemy2dkwtgq"));
    }

    async global::System.Threading.Tasks.Task Because() => _result = await _scenario.Execute(new global::Shop.Shop.Customers.RegisterCustomer.RegisterCustomer(new global::Shop.Common.CustomerName("Ada Lovelace")));

    [global::Xunit.FactAttribute] void should_succeed() => _result.ShouldBeSuccessful();
    [global::Xunit.FactAttribute] async global::System.Threading.Tasks.Task should_have_appended_customer_registered() => await _scenario.ShouldHaveAppendedEvent<global::Shop.Shop.Customers.RegisterCustomer.RegisterCustomer, global::Shop.Shop.Customers.RegisterCustomer.CustomerRegistered>(new global::Shop.Common.CustomerId("cus_01m4t7txm0hrn5rwemy2dkwtgq"), @event => @event.Name == new global::Shop.Common.CustomerName("Ada Lovelace"));
    [global::Xunit.FactAttribute] void should_return_the_expected_response() => ((global::Cratis.Arc.Commands.CommandResult<global::Shop.Common.CustomerId>)_result).Response.ShouldEqual(new global::Shop.Common.CustomerId("cus_01m4t7txm0hrn5rwemy2dkwtgq"));

    public void Dispose() => _scenario.Dispose();
}

#endif
