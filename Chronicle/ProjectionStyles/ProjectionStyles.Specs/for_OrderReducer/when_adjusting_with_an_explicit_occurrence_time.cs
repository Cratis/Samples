// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;

namespace ProjectionStyles.Specs.for_OrderReducer;

public class when_adjusting_with_an_explicit_occurrence_time : Specification
{
    readonly DateTimeOffset _occurred = new(2026, 6, 1, 12, 0, 0, TimeSpan.Zero);
    readonly ProductId _productId = ProductId.New();
    ReducedOrder _current;
    ReducedOrder _result;

    void Establish() => _current = new(OrderId.New(), "Sample customer", false, 1, _occurred.AddDays(-1), [new(_productId, 2, null)]);
    void Because() => _result = new OrderReducer().Adjusted(new ItemQuantityAdjusted(_productId, 5), _current, EventContext.Empty with { Occurred = _occurred })!;

    [Fact] void should_use_the_event_context_occurrence_time() => _result.LastUpdated.ShouldEqual(_occurred);
    [Fact] void should_not_mutate_the_previous_state() => _current.Lines.Single().Quantity.ShouldEqual(2);
}
