using Cratis.Chronicle.EventSequences;
using Cratis.DependencyInjection;
using Cratis.Json;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Quickstart;
using Quickstart.Common;

MongoDBDefaults.Initialize();

#region Snippet:Quickstart-AspNetCore-WebApplicationBuilder
var builder = WebApplication.CreateBuilder(args)
    .AddCratisChronicle(options => options.EventStore = "Quickstart");
#endregion Snippet:Quickstart-AspNetCore-WebApplicationBuilder

#region Snippet:Quickstart-AspNetCore-MongoSetup
builder.Services.AddSingleton<IMongoClient>(new MongoClient("mongodb://localhost:27017"));
builder.Services.AddSingleton(provider => provider.GetRequiredService<IMongoClient>().GetDatabase("Quickstart"));
builder.Services.AddTransient(provider => provider.GetRequiredService<IMongoDatabase>().GetCollection<User>("book"));
#endregion Snippet:Quickstart-AspNetCore-MongoSetup

builder.Services.AddTransient(provider => provider.GetRequiredService<IMongoDatabase>().GetCollection<User>("borrowedBook"));
builder.Services.AddTransient(provider => provider.GetRequiredService<IMongoDatabase>().GetCollection<User>("overdueBook"));
builder.Services.AddTransient(provider => provider.GetRequiredService<IMongoDatabase>().GetCollection<User>("reservedBook"));
builder.Services.AddTransient(provider => provider.GetRequiredService<IMongoDatabase>().GetCollection<User>("users"));

#region Snippet:Quickstart-AspNetCore-ArtifactsServices
builder.Services.AddTransient<UsersReactor>();
builder.Services.AddTransient<BooksReducer>();
builder.Services.AddTransient<BorrowedBooksProjection>();
builder.Services.AddTransient<OverdueBooksProjection>();
builder.Services.AddTransient<ReservedBooksProjection>();
#endregion Snippet:Quickstart-AspNetCore-ArtifactsServices

#region Snippet:Quickstart-AspNetCore-Services
builder.Services.AddTransient<Books>();
builder.Services.AddTransient<BorrowedBooks>();
builder.Services.AddTransient<OverdueBooks>();
builder.Services.AddTransient<ReservedBooks>();
builder.Services.AddTransient<Users>();
builder.Services.AddTransient<DemoData>();
#endregion Snippet:Quickstart-AspNetCore-Services

#region Snippet:Quickstart-AspNetCore-WebApplication
var app = builder.Build();
app.UseCratisChronicle();
#endregion Snippet:Quickstart-AspNetCore-WebApplication

app.MapGet("/api/demo-data", async ([FromServices] DemoData demoData) => await demoData.Initialize());

await app.RunAsync();
