// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

// Force invariant culture for the Backend
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;

var builder = WebApplication.CreateBuilder(args)
    .UseCratisApplicationModel()
    .AddCratisChronicle(options => options.EventStore = "Library");
builder.UseCratisMongoDB();
builder.Services.AddControllers();
builder.Services.AddMvc();
builder.Services.Configure<ApiBehaviorOptions>(_ => _.SuppressModelStateInvalidFilter = true);

var app = builder.Build();
app
    .UseCratisChronicle()
    .UseCratisApplicationModel()
    .UseDefaultFiles()
    .UseStaticFiles();

app.UseWebSockets();
app.MapControllers();

await app.RunAsync();

public partial class Program;
