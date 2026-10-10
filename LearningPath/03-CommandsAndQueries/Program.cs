// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

var builder = WebApplication.CreateBuilder(args);

// Arc for commands and queries, Chronicle for the events underneath.
// The event store, Chronicle connection, and API routes are configured in appsettings.json.
builder.AddCratis();

var app = builder.Build();
app.UseCratis();

await app.RunAsync();
