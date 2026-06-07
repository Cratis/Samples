// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using AspNetCoreArcBuilderExtensions = Microsoft.AspNetCore.Builder.ArcBuilderExtensions;

// Force invariant culture for the Backend
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;

var builder = WebApplication.CreateBuilder(args)
    .AddCratisArc(
        options =>
        {
            options.GeneratedApis.RoutePrefix = "api";
            options.GeneratedApis.IncludeCommandNameInRoute = false;
            options.GeneratedApis.SegmentsToSkipForRoute = 1;
        },
        arcBuilder => AspNetCoreArcBuilderExtensions.WithChronicle(arcBuilder))
    .AddCratisChronicle(options => options.EventStore = "Members");
builder.UseCratisMongoDB();
builder.Services.AddControllers();
builder.Services.AddMvc();
builder.Services.AddSwaggerGen(options => options.AddConcepts());
builder.Services.Configure<ApiBehaviorOptions>(_ => _.SuppressModelStateInvalidFilter = true);

var app = builder.Build();

app.UseRouting();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseWebSockets();
app.MapControllers();
app.UseCratisArc();
app.UseCratisChronicle();

app.UseSwagger();
app.UseSwaggerUI();
app.MapFallbackToFile("/index.html");

await app.RunAsync();
