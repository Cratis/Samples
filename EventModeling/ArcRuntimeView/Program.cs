// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc;
using Cratis.Arc.MongoDB;
using Cratis.Chronicle.AspNetCore;
using Cratis.Chronicle.Connections;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddCratisArc(configureBuilder: arc => arc.WithChronicle().WithMongoDB());

// With these released packages, Arc convention-binds IChronicleConnection to a
// constructor that needs primitive arguments. Let Chronicle create its configured connection instead.
builder.Services.RemoveAll<IChronicleConnection>();
builder.Services.Configure<ChronicleAspNetCoreOptions>(builder.Configuration.GetSection("Cratis:Chronicle"));
builder.Services.AddControllers();

var app = builder.Build();
app.UseRouting();
app.UseCratisArc();
app.UseCratisChronicle();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapCratisEventModel();
}

await app.RunAsync();
