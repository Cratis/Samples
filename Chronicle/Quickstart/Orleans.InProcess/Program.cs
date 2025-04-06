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
var builder = WebApplication.CreateBuilder(args);
#endregion Snippet:Quickstart-AspNetCore-WebApplicationBuilder

builder.Host.UseOrleans(static siloBuilder =>
{
    siloBuilder.UseLocalhostClustering();
    siloBuilder.AddChronicleToSilo();
    siloBuilder.AddMemoryGrainStorage("urls");
});

builder.Services.AddOpenApi();

#region Snippet:Quickstart-AspNetCore-ServiceValidation
builder.Host.UseDefaultServiceProvider(_ =>
   {
       _.ValidateScopes = false;
       _.ValidateOnBuild = false;
   });
#endregion Snippet:Quickstart-AspNetCore-ServiceValidation

builder.AddMongoDBServices();
builder.AddArtifacts();

#region Snippet:Quickstart-AspNetCore-WebApplication
var app = builder.Build();
app.UseCratisChronicle();
#endregion Snippet:Quickstart-AspNetCore-WebApplication

app.MapOpenApi();
app.MapScalarApiReference();

app.MapApi();

await app.RunAsync();
