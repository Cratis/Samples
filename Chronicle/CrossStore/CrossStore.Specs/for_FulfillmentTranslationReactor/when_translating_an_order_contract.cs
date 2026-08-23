// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CrossStore.Specs.for_FulfillmentTranslationReactor;

public class when_translating_an_order_contract : Specification
{
    FulfillmentOrderReceived _result;

    void Because() => _result = new FulfillmentTranslationReactor().Requested(new("SKU-42", 3));

    [Fact] void should_translate_to_a_target_owned_fact() => _result.ShouldBeOfExactType<FulfillmentOrderReceived>();
    [Fact] void should_translate_the_product_identifier() => _result.Sku.ShouldEqual(new FulfillmentSku("SKU-42"));
    [Fact] void should_translate_the_quantity() => _result.Quantity.ShouldEqual(new UnitsToFulfill(3));
}
