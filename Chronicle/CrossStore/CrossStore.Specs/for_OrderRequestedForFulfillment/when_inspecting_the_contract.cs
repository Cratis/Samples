// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CrossStore.Specs.for_OrderRequestedForFulfillment;

public class when_inspecting_the_contract : Specification
{
    string[] _propertyNames;

    void Because() => _propertyNames =
    [
        .. typeof(OrderRequestedForFulfillment)
            .GetProperties()
            .Select(_ => _.Name)
    ];

    [Fact] void should_include_only_fulfillment_data() => _propertyNames.ShouldContainOnly(nameof(OrderRequestedForFulfillment.Sku), nameof(OrderRequestedForFulfillment.Quantity));
    [Fact] void should_not_expose_the_source_owned_buyer() => _propertyNames.ShouldNotContain(nameof(OrderPlaced.Buyer));
}
