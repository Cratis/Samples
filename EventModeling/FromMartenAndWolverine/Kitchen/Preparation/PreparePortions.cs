// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Marten;

namespace FromMartenAndWolverine.Kitchen.Preparation;

/// <summary>
/// Records a completed round of preparation.
/// </summary>
/// <param name="Id">The batch identity.</param>
/// <param name="Portions">The additional portions.</param>
public record PreparePortions(Guid Id, int Portions);

/// <summary>
/// Handles preparation through an explicit event append.
/// </summary>
public static class PreparePortionsHandler
{
    /// <summary>
    /// Appends and saves the preparation fact.
    /// </summary>
    /// <param name="command">The preparation details.</param>
    /// <param name="session">The event session.</param>
    /// <returns>A task for the persisted preparation.</returns>
    public static async Task Handle(PreparePortions command, IDocumentSession session)
    {
        session.Events.Append(command.Id, new PortionsPrepared(command.Portions));
        await session.SaveChangesAsync();
    }
}
