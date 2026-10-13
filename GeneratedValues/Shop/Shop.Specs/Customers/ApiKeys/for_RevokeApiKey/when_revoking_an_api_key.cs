// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.EventSequences;
using Shop.Customers.ApiKeys;

namespace Shop.Specs.Customers.ApiKeys.for_RevokeApiKey;

public class when_revoking_an_api_key : Specification
{
    readonly CommandScenario<RevokeApiKey> _scenario = new();
    readonly CustomerId _customerId = new("cus_01m4t7txm0hrn5rwemy2dkwtgq");
    readonly ApiKey _apiKey = new("AQIDBAUGBwgJCgsMDQ4PEBESExQVFhcYGRobHB0eHyA");
    CommandResult _result = null!;
    IDictionary<string, string> _causation = null!;

    async Task Because()
    {
        _result = await _scenario.Execute(new RevokeApiKey(_customerId, _apiKey));
        var appended = (List<AppendedEventWithResult>)_scenario.Context[ChronicleCommandScenarioExtender.AppendedEventsKey];
        _causation = appended.Single().Event.Context.Causation.Last().Properties;
    }

    void Destroy() => _scenario.Dispose();

    [Fact] void should_succeed() => _result.ShouldBeSuccessful();
    [Fact] void should_record_the_customer_on_the_causation() => _causation["customerId"].ShouldEqual(_customerId.Value);
    [Fact] void should_leave_the_api_key_off_the_causation() => _causation.ContainsKey("apiKey").ShouldBeFalse();
    [Fact] void should_not_write_the_api_key_anywhere_on_the_causation() => _causation.Values.Any(value => value.Contains(_apiKey.Value, StringComparison.Ordinal)).ShouldBeFalse();
}
