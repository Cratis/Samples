// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Wolverine;
using Wolverine.Http;

namespace FromMartenAndWolverine.Kitchen.Preparation;

/// <summary>
/// Accepts a round of preparation over HTTP.
/// </summary>
public static class PreparePortionsEndpoint
{
    /// <summary>
    /// Sends preparation details to their Wolverine handler.
    /// </summary>
    /// <param name="command">The preparation details.</param>
    /// <param name="bus">The local message bus.</param>
    /// <returns>A task for the completed handler.</returns>
    [WolverinePost("/batches/prepare")]
    public static Task Prepare(PreparePortions command, IMessageBus bus) => bus.InvokeAsync(command);
}
