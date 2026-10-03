// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Wolverine.Marten;

namespace FromMartenAndWolverine.Kitchen.Serving;

/// <summary>
/// Moves a fully prepared batch to the serving table.
/// </summary>
/// <param name="Id">The batch stream identity.</param>
public record ServeBatch(Guid Id);

/// <summary>
/// Uses Wolverine's aggregate workflow rather than an explicit document session.
/// </summary>
public static class ServeBatchHandler
{
    /// <summary>
    /// Emits a serving fact only for a ready, not-yet-served batch.
    /// </summary>
    /// <param name="command">The batch to serve.</param>
    /// <param name="batch">The aggregate loaded by Wolverine.</param>
    /// <returns>The event to append, or no events if the batch is not ready or was already served.</returns>
    [AggregateHandler]
    public static IEnumerable<object> Handle(ServeBatch command, SoupBatch batch)
    {
        if (!batch.Served && batch.PreparedPortions >= batch.TargetPortions)
        {
            yield return new BatchServed(batch.PreparedPortions);
        }
    }
}
