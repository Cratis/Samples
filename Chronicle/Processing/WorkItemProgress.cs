// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;
using Cratis.Chronicle.Reducers;

namespace Processing;

/// <summary>
/// Represents progress accumulated from a work item's event stream.
/// </summary>
/// <param name="Id">The event source identifier.</param>
/// <param name="PlannedPoints">The number of points planned for the work item.</param>
/// <param name="CompletedPoints">The accumulated completed points, capped at the plan.</param>
/// <param name="RemainingPoints">The number of planned points that remain.</param>
/// <param name="LastSequenceNumber">The sequence number of the last event processed.</param>
public record WorkItemProgress(
    WorkItemId Id,
    WorkPoints PlannedPoints,
    WorkPoints CompletedPoints,
    WorkPoints RemainingPoints,
    EventSequenceNumber LastSequenceNumber);

/// <summary>
/// Folds work item events into accumulated progress.
/// </summary>
/// <remarks>
/// A reducer is required because capped completion and remaining work both depend on prior state;
/// projection attributes and fluent setters cannot express this coordinated transition.
/// </remarks>
public class WorkItemProgressReducer : IReducerFor<WorkItemProgress>
{
    /// <summary>
    /// Initializes progress from the work item's plan.
    /// </summary>
    /// <param name="event">The event that opened the work item.</param>
    /// <param name="current">The current state, which is not used for the opening event.</param>
    /// <param name="context">The event context.</param>
    /// <returns>The initialized work item progress.</returns>
    public WorkItemProgress Opened(WorkItemOpened @event, WorkItemProgress? current, EventContext context)
    {
        var workItemId = (WorkItemId)Guid.Parse(context.EventSourceId);
        return new(
            workItemId,
            @event.PlannedPoints,
            WorkPoints.NotSet,
            @event.PlannedPoints,
            context.SequenceNumber);
    }

    /// <summary>
    /// Applies a progress update to the prior accumulated state.
    /// </summary>
    /// <param name="event">The progress update.</param>
    /// <param name="current">The current accumulated state, or <see langword="null"/> before opening.</param>
    /// <param name="context">The event context.</param>
    /// <returns>The next accumulated state, or <see langword="null"/> when no opening event exists.</returns>
    public WorkItemProgress? Recorded(ProgressRecorded @event, WorkItemProgress? current, EventContext context)
    {
        if (current is null)
        {
            return null;
        }

        var completedPoints = Math.Min(
            current.PlannedPoints.Value,
            current.CompletedPoints.Value + @event.Points.Value);
        return current with
        {
            CompletedPoints = completedPoints,
            RemainingPoints = current.PlannedPoints.Value - completedPoints,
            LastSequenceNumber = context.SequenceNumber
        };
    }
}
