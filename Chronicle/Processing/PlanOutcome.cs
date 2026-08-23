// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Processing;

/// <summary>
/// Describes whether completed work met its plan.
/// </summary>
public enum PlanOutcome
{
    /// <summary>
    /// No outcome has been recorded.
    /// </summary>
    NotSet = 0,

    /// <summary>
    /// Completed work did not meet the plan.
    /// </summary>
    Missed = 1,

    /// <summary>
    /// Completed work met or exceeded the plan.
    /// </summary>
    Met = 2
}
