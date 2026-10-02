// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FromMartenAndWolverine.Kitchen.Cards;
using JasperFx.Events.Projections;
using Marten;
using Wolverine;
using Wolverine.Http;
using Wolverine.Marten;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseWolverine();
builder.Services.AddWolverineHttp();
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Kitchen")!);
    options.Projections.Add<BatchCardProjection>(ProjectionLifecycle.Inline);
}).IntegrateWithWolverine();

var app = builder.Build();
app.MapWolverineEndpoints();
await app.RunAsync();
