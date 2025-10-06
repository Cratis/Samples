// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using System.Reflection;
using Cratis.Applications.Swagger;

// Force invariant culture for the Backend
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;

var builder = WebApplication.CreateBuilder(args)
    .UseCratisApplicationModel(options =>
    {
        options.GeneratedApis.RoutePrefix = "api";
        options.GeneratedApis.IncludeCommandNameInRoute = false;
        options.GeneratedApis.SegmentsToSkipForRoute = 1;
    })
    .AddCratisChronicle(options => options.EventStore = "Library");
builder.UseCratisMongoDB();
builder.Services.AddControllers();
builder.Services.AddMvc();
builder.Services.AddSwaggerGen(options => options.AddConcepts());
builder.Services.Configure<ApiBehaviorOptions>(_ => _.SuppressModelStateInvalidFilter = true);

var app = builder.Build();
app
    .UseCratisChronicle()
    .UseCratisApplicationModel()
    .UseDefaultFiles()
    .UseStaticFiles();

app.UseWebSockets();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger");
    const string resourceName = "Main.SwaggerDark.css";
    using var stream = Assembly.GetEntryAssembly()!.GetManifestResourceStream(resourceName);
    if (stream is not null)
    {
        using var streamReader = new StreamReader(stream);
        var styles = streamReader.ReadToEnd();
        options.HeadContent = $"{options.HeadContent}<style>{styles}</style>";
    }
});
app.MapFallbackToFile("/index.html");

await app.RunAsync();

public partial class Program;
