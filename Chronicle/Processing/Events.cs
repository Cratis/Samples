// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;

namespace Processing;

/// <summary>
/// The event that records the opening plan for a work item.
/// </summary>
/// <param name="Title">The work item title.</param>
/// <param name="PlannedPoints">The number of points planned for the work item.</param>
[EventType]
public record WorkItemOpened(WorkItemTitle Title, WorkPoints PlannedPoints);

/// <summary>
/// The event that records completed points against a work item.
/// </summary>
/// <param name="Points">The number of points completed by this update.</param>
[EventType]
public record ProgressRecorded(WorkPoints Points);

/// <summary>
/// The event that records the final delivery totals for a work item.
/// </summary>
/// <param name="CompletedPoints">The final number of completed points.</param>
/// <param name="PlannedPoints">The number of points that were planned.</param>
[EventType]
public record WorkItemCompleted(WorkPoints CompletedPoints, WorkPoints PlannedPoints);

/// <summary>
/// The event produced by the completion reactor with a deterministic delivery summary.
/// </summary>
/// <param name="Summary">The human-readable delivery summary.</param>
/// <param name="Outcome">Whether the completed points met or exceeded the plan.</param>
[EventType]
public record CompletionSummarized(CompletionSummary Summary, PlanOutcome Outcome);
