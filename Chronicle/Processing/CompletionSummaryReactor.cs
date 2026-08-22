// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Reactors;

namespace Processing;

/// <summary>
/// Produces a deterministic summary when a work item completes.
/// </summary>
/// <remarks>
/// The result depends only on the triggering event, while <see cref="OnceOnlyAttribute"/> prevents
/// the follow-up fact from being repeated during replay. Chronicle handles the returned event without
/// direct event-log access in the reactor.
/// </remarks>
public class CompletionSummaryReactor : IReactor
{
    /// <summary>
    /// Summarizes the completed work against its plan.
    /// </summary>
    /// <param name="event">The completion event.</param>
    /// <returns>The deterministic completion summary.</returns>
    [OnceOnly]
    public CompletionSummarized Completed(WorkItemCompleted @event)
    {
        var metPlan = @event.CompletedPoints.Value >= @event.PlannedPoints.Value;
        var result = metPlan ? "met" : "did not meet";
        return new(
            $"Completed {@event.CompletedPoints.Value} of {@event.PlannedPoints.Value} planned points and {result} the plan.",
            metPlan ? PlanOutcome.Met : PlanOutcome.Missed);
    }
}
