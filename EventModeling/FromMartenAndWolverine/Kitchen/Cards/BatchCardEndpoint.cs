// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Marten;
using Wolverine.Http;

namespace FromMartenAndWolverine.Kitchen.Cards;

/// <summary>
/// Exposes the volunteer-facing card over HTTP.
/// </summary>
public static class BatchCardEndpoint
{
    /// <summary>
    /// Reads the current preparation progress.
    /// </summary>
    /// <param name="id">The batch identity.</param>
    /// <param name="session">The read-only document session.</param>
    /// <returns>The current card, if present.</returns>
    [WolverineGet("/batches/{id}")]
    public static Task<BatchCard?> Get(Guid id, IQuerySession session) => session.LoadAsync<BatchCard>(id);
}
