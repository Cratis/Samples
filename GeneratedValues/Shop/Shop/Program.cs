// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.MongoDB;

var builder = WebApplication.CreateBuilder(args);
builder.AddCratis(
    configureArcBuilder: arc => arc.WithMongoDB(),
    configureChronicleBuilder: chronicle => chronicle.WithCamelCaseNamingPolicy());

var app = builder.Build();
app.UseCratis();

await app.RunAsync();
