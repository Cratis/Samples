// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Tenancy;
using AspNetCoreArcBuilderExtensions = Microsoft.AspNetCore.Builder.ArcBuilderExtensions;

var builder = WebApplication.CreateBuilder(args)
    .AddCratisArc(
        options =>
        {
            options.UseHeaderTenancy();
            options.GeneratedApis.RoutePrefix = "api";
            options.GeneratedApis.SegmentsToSkipForRoute = 1;
        },
        arcBuilder => AspNetCoreArcBuilderExtensions.WithChronicle(
            arcBuilder,
            options => options.EventStore = "MultiTenancy"));

builder.UseCratisMongoDB(options =>
{
    options.Server = "mongodb://localhost:27017";
    options.Database = "MultiTenancy";
});

var app = builder.Build();

app.UseRouting();
app.UseWebSockets();
app.UseCratisArc();
app.UseCratisChronicle();

app.MapGet("/", () => Results.Ok(new
{
    Sample = "Arc and Chronicle multi-tenancy",
    TenantHeader = "x-cratis-tenant-id",
    DefaultNamespace = "Default"
}));

await app.RunAsync();
