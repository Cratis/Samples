using Cratis.Chronicle.EventSequences;
using Cratis.DependencyInjection;
using Cratis.Json;
using MongoDB.Driver;
using Quickstart.Common;

MongoDBDefaults.Initialize();

var builder = WebApplication.CreateBuilder(args)
    .AddCratisChronicle(options => options.EventStore = "Quickstart");

builder.Host.UseDefaultServiceProvider(_ =>
   {
       _.ValidateScopes = false;
       _.ValidateOnBuild = false;
   });

builder.Services
    .AddBindingsByConvention()
    .AddSelfBindings();

builder.Services.AddSingleton<IMongoClient>(new MongoClient("mongodb://localhost:27017"));
builder.Services.AddSingleton(provider => provider.GetRequiredService<IMongoClient>().GetDatabase("Quickstart"));
builder.Services.AddTransient(provider => provider.GetRequiredService<IMongoDatabase>().GetCollection<User>("book"));
builder.Services.AddTransient(provider => provider.GetRequiredService<IMongoDatabase>().GetCollection<User>("borrowedBook"));
builder.Services.AddTransient(provider => provider.GetRequiredService<IMongoDatabase>().GetCollection<User>("overdueBook"));
builder.Services.AddTransient(provider => provider.GetRequiredService<IMongoDatabase>().GetCollection<User>("reservedBook"));
builder.Services.AddTransient(provider => provider.GetRequiredService<IMongoDatabase>().GetCollection<User>("users"));

var app = builder.Build();
app.UseCratisChronicle();

app.MapGet("/api/demo-data", async (DemoData demoData) => await demoData.Initialize());

await app.RunAsync();
