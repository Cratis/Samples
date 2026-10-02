// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Marten;

namespace FromMartenAndWolverine.Kitchen.Planning;

/// <summary>
/// Plans one batch for the community meal.
/// </summary>
/// <param name="Id">The new stream identity.</param>
/// <param name="Recipe">The soup to prepare.</param>
/// <param name="TargetPortions">The intended number of portions.</param>
public record PlanBatch(Guid Id, string Recipe, int TargetPortions);

/// <summary>
/// Handles batch planning through an explicit Marten session.
/// </summary>
public static class PlanBatchHandler
{
    /// <summary>
    /// Starts and saves a new batch stream.
    /// </summary>
    /// <param name="command">The plan.</param>
    /// <param name="session">The event session.</param>
    /// <returns>A task for the persisted plan.</returns>
    public static async Task Handle(PlanBatch command, IDocumentSession session)
    {
        session.Events.StartStream<SoupBatch>(command.Id, new BatchPlanned(command.Recipe, command.TargetPortions));
        await session.SaveChangesAsync();
    }
}
