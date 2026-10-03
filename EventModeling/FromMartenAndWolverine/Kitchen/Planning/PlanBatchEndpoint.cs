// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Wolverine;
using Wolverine.Http;

namespace FromMartenAndWolverine.Kitchen.Planning;

/// <summary>
/// Accepts a community meal plan over HTTP.
/// </summary>
public static class PlanBatchEndpoint
{
    /// <summary>
    /// Sends the plan to its Wolverine handler.
    /// </summary>
    /// <param name="command">The batch plan.</param>
    /// <param name="bus">The local message bus.</param>
    /// <returns>A task for the completed handler.</returns>
    [WolverinePost("/batches")]
    public static Task Plan(PlanBatch command, IMessageBus bus) => bus.InvokeAsync(command);
}
