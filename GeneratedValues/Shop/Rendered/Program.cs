// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.MongoDB;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();
builder.AddCratis(
    configureArcBuilder: arc => arc.WithMongoDB(),
    configureChronicleBuilder: chronicle => chronicle.WithCamelCaseNamingPolicy());
Shop.GeneratedPolicies.Registration.Register(builder.Services);
ConfigureServices(builder);

var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCratis();
app.MapHealthChecks("/healthz");
app.MapFallbackToFile("/index.html");
ConfigureApplication(app);

await app.RunAsync();

// Optional partial methods disappear when Customizations/Program.cs supplies no implementation.
public partial class Program
{
    static partial void ConfigureServices(WebApplicationBuilder builder);
    static partial void ConfigureApplication(WebApplication app);
}
