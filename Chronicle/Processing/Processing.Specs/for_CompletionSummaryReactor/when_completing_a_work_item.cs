// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Processing.Specs.for_CompletionSummaryReactor;

public class when_completing_a_work_item : Specification
{
    CompletionSummarized _result;

    void Because() => _result = new CompletionSummaryReactor().Completed(new WorkItemCompleted(8, 8));

    [Fact]
    void should_produce_the_expected_summary() =>
        _result.ShouldEqual(new CompletionSummarized("Completed 8 of 8 planned points and met the plan.", PlanOutcome.Met));
}
