// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Chronicle.Backend;

var builder = WebApplication.CreateBuilder(args)
    .AddCratisChronicle(options => options.EventStore = ChronicleConfiguration.EventStore);

builder.Services.AddScoped<ChronicleReadiness>();
builder.Services.AddScoped<Timeline>();

var app = builder.Build();
app.UseCratisChronicle();
app.MapTimelineEndpoints();

await app.RunAsync();
