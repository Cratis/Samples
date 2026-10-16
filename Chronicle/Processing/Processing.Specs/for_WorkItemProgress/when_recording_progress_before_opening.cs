// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Processing.Specs.for_WorkItemProgress;

public class when_recording_progress_before_opening : Specification
{
    WorkItemProgress? _result;

    void Because() => _result = WorkItemProgress.Reduce(new ProgressRecorded(4), null, EventContext.Empty);

    [Fact] void should_leave_the_model_absent() => _result.ShouldBeNull();
}
