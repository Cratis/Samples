// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Security.AccessControl;
using Cratis.Chronicle;
using Cratis.Chronicle.EventSequences;
using Cratis.Chronicle.Setup;
using Cratis.DependencyInjection;
using Cratis.Json;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Orleans.Metadata;
using Quickstart;
using Quickstart.Common;
using Quickstart.Common.AspNetCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.UseCratisApplicationModel();
builder.AddCratisChronicle();
builder.UseCratisMongoDB(
    mongo =>
    {
        mongo.Server = "mongodb://localhost:27017";
        mongo.Database = "Quickstart";
    });

builder.Services
    .AddCratisApplicationModelMeter()
    .AddChronicleMeters();

builder.Host.UseOrleans(static siloBuilder =>
{
    siloBuilder.UseLocalhostClustering();
    siloBuilder.AddCratisChronicle(
        options => options.EventStoreName = "Quickstart",
        builder => builder.WithMongoDB("mongodb://localhost:27017"));
    siloBuilder.AddMemoryGrainStorage("urls");
});

builder.Services.AddOpenApi();

builder.AddMongoDBServices();
builder.AddArtifacts();

var app = builder.Build();
app.UseCratisChronicle();

app.MapOpenApi();
app.MapScalarApiReference();

app.MapApi();

await app.RunAsync();
