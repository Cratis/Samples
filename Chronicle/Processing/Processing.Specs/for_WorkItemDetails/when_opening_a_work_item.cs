// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Processing.Specs.for_WorkItemDetails;

public class when_opening_a_work_item : Specification
{
    bool _isBoundToOpeningEvent;

    void Because() =>
        _isBoundToOpeningEvent = typeof(WorkItemDetails).IsDefined(typeof(FromEventAttribute<WorkItemOpened>), false);

    [Fact] void should_bind_the_model_to_the_opening_event() => _isBoundToOpeningEvent.ShouldBeTrue();
}
