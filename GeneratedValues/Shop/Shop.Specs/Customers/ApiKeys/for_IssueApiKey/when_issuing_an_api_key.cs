// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Shop.Customers.ApiKeys;

namespace Shop.Specs.Customers.ApiKeys.for_IssueApiKey;

public class when_issuing_an_api_key : Specification
{
    readonly CommandScenario<IssueApiKey> _scenario = new();
    readonly CustomerId _customerId = new("cus_01m4t7txm0hrn5rwemy2dkwtgq");
    readonly ApiKey _apiKey = new("AQIDBAUGBwgJCgsMDQ4PEBESExQVFhcYGRobHB0eHyA");
    CommandResult _result = null!;

    void Establish() => _scenario.PinGeneratedValue("apiKey", _apiKey);
    async Task Because() => _result = await _scenario.Execute(new IssueApiKey(_customerId));
    void Destroy() => _scenario.Dispose();

    [Fact] void should_succeed() => _result.ShouldBeSuccessful();
    [Fact] void should_return_the_pinned_api_key() => ((CommandResult<ApiKey>)_result).Response.ShouldEqual(_apiKey);
    [Fact] Task should_record_that_a_key_was_issued() => _scenario.ShouldHaveAppendedEvent<IssueApiKey, ApiKeyIssued>(_customerId);
}
