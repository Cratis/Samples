// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace ProjectionStyles.Specs.for_ReducedOrder;

public class when_cancelling_an_order : given.an_order<ReducedOrder>
{
    async Task Establish() => await _scenario.Given.ForEventSource(_orderId).Events(
        new OrderPlaced("Sample customer"), new ItemAddedToOrder(_productId, 2), new OrderCancelled());

    [Fact] void should_remove_the_order() => _scenario.Instance.ShouldBeNull();
}
