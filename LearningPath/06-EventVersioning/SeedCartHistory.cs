// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;
using Cratis.Chronicle.EventSequences;

namespace LearningPath.EventVersioning.Carts;

/// <summary>
/// Supplies one generation-1 fact to stand in for history recorded before unit prices existed.
/// </summary>
public static class SeedCartHistory
{
    /// <summary>
    /// Gets the fixed cart used to demonstrate old history.
    /// </summary>
    public static readonly CartId OldCart = new(new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6));

    /// <summary>
    /// Appends the old fact only when this cart has no history. Run one sample instance at a time.
    /// </summary>
    /// <param name="eventLog">The event log for this step's event store.</param>
    /// <returns>Whether the old history exists or was successfully appended.</returns>
    public static async Task<bool> Ensure(IEventLog eventLog)
    {
        // This is learning data, not a production migration or a rewrite of a stored event.
        var eventSourceId = (EventSourceId)OldCart;
        if (await eventLog.HasEventsFor(eventSourceId))
        {
            return true;
        }

        var result = await eventLog.Append(eventSourceId, new ItemAddedToCartV1("Coffee beans", 2));
        return result.IsSuccess;
    }
}
