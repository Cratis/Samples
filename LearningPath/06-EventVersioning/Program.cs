// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.EventSequences;
using LearningPath.EventVersioning.Carts;

var builder = WebApplication.CreateBuilder(args);

// Arc for commands and queries, Chronicle for the events underneath.
// The event store, Chronicle connection, and API routes are configured in appsettings.json.
builder.AddCratis();

await using var app = builder.Build();
app.UseCratis();

// Start Chronicle's connection and artifact registration before appending the learning history.
await app.StartAsync();
await using (var scope = app.Services.CreateAsyncScope())
{
    if (!await SeedCartHistory.Ensure(scope.ServiceProvider.GetRequiredService<IEventLog>()))
    {
        await Console.Error.WriteLineAsync("Chronicle rejected the generation-1 learning history. Check the Chronicle server.");
        await app.StopAsync();
        return 1;
    }
}

await app.WaitForShutdownAsync();
return 0;
