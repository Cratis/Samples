// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Processing.Specs.for_WorkItemProgress;

public class when_recording_progress : Specification
{
    readonly WorkItemId _workItemId = WorkItemId.New();
    WorkItemProgress _result;

    void Because()
    {
        var reducer = new WorkItemProgressReducer();
        _result = reducer.Opened(new WorkItemOpened("Prepare release", 10), null, ContextFor(0));
        _result = reducer.Recorded(new ProgressRecorded(4), _result, ContextFor(1));
        _result = reducer.Recorded(new ProgressRecorded(5), _result, ContextFor(2));
        _result = reducer.Recorded(new ProgressRecorded(3), _result, ContextFor(3));
    }

    [Fact]
    void should_fold_every_update_into_the_capped_progress() =>
        _result.ShouldEqual(new WorkItemProgress(_workItemId, 10, 10, WorkPoints.NotSet, new EventSequenceNumber(3)));

    EventContext ContextFor(ulong sequenceNumber) =>
        EventContext.Empty with
        {
            EventSourceId = _workItemId,
            SequenceNumber = new EventSequenceNumber(sequenceNumber)
        };
}
