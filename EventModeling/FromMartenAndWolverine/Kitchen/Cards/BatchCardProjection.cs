// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Marten.Events.Aggregation;

namespace FromMartenAndWolverine.Kitchen.Cards;

/// <summary>
/// Updates the volunteer-facing card inline with event writes.
/// </summary>
public class BatchCardProjection : SingleStreamProjection<BatchCard, Guid>
{
    /// <summary>
    /// Creates the card from the plan.
    /// </summary>
    /// <param name="@event">The batch plan.</param>
    /// <returns>The initial card.</returns>
    public static BatchCard Create(BatchPlanned @event) => new()
    {
        Recipe = @event.Recipe,
        TargetPortions = @event.TargetPortions
    };

    /// <summary>
    /// Adds newly prepared portions to the card.
    /// </summary>
    /// <param name="@event">The additional portions.</param>
    /// <param name="card">The current card.</param>
    public static void Apply(PortionsPrepared @event, BatchCard card) => card.PreparedPortions += @event.Portions;

    /// <summary>
    /// Shows that volunteers can start serving.
    /// </summary>
    /// <param name="@event">The serving fact.</param>
    /// <param name="card">The current card.</param>
    public static void Apply(BatchServed @event, BatchCard card) => card.Served = true;
}
