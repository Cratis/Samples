// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Security.AccessControl;
using Cratis.Chronicle;
using Cratis.Chronicle.EventSequences;
using Cratis.DependencyInjection;
using Cratis.Json;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Quickstart;
using Quickstart.Common;
using Quickstart.Common.AspNetCore;
using Scalar.AspNetCore;

MongoDBDefaults.Initialize();

#region Snippet:Quickstart-AspNetCore-WebApplicationBuilder
var builder = WebApplication.CreateBuilder(args)
    .AddCratisChronicle(options => options.EventStore = "Quickstart");
#endregion Snippet:Quickstart-AspNetCore-WebApplicationBuilder

builder.Services.AddSingleton(Globals.JsonSerializerOptions);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.AddMongoDBServices();
builder.AddArtifacts();

#region Snippet:Quickstart-AspNetCore-WebApplication
var app = builder.Build();
app.UseCratisChronicle();
#endregion Snippet:Quickstart-AspNetCore-WebApplication

app.MapControllers();
app.MapOpenApi();
app.MapScalarApiReference();

app.MapApi();

await app.RunAsync();
