// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Projections.ModelBound;

namespace Processing;

/// <summary>
/// Provides the stable identity and plan for a work item.
/// </summary>
/// <param name="Id">The event source identifier.</param>
/// <param name="Title">The work item title.</param>
/// <param name="PlannedPoints">The number of points planned for the work item.</param>
[FromEvent<WorkItemOpened>]
public record WorkItemDetails(WorkItemId Id, WorkItemTitle Title, WorkPoints PlannedPoints);
