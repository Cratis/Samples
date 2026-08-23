// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Arc.React.Ideas.Board;

var builder = WebApplication.CreateBuilder(args);

builder.AddCratisArc(options =>
{
    options.GeneratedApis.RoutePrefix = "api";
    options.GeneratedApis.IncludeCommandNameInRoute = false;
    options.GeneratedApis.SegmentsToSkipForRoute = 2;
});
builder.Services.AddSingleton<IdeaStore>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseWebSockets();
app.UseCratisArc();
app.MapFallbackToFile("index.html");

await app.RunAsync();
