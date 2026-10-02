// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Wolverine;
using Wolverine.Http;

namespace FromMartenAndWolverine.Kitchen.Serving;

/// <summary>
/// Accepts a request to open the serving table.
/// </summary>
public static class ServeBatchEndpoint
{
    /// <summary>
    /// Invokes the aggregate handler synchronously.
    /// </summary>
    /// <param name="command">The batch to serve.</param>
    /// <param name="bus">The local message bus.</param>
    /// <returns>A task for the completed handler.</returns>
    [WolverinePost("/batches/serve")]
    public static Task Serve(ServeBatch command, IMessageBus bus) => bus.InvokeAsync(command);
}
