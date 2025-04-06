// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Quickstart.Common.AspNetCore;

public static class MongoDBServices
{
    public static void AddMongoDBServices(this WebApplicationBuilder builder)
    {
        #region Snippet:Quickstart-AspNetCore-MongoSetup
        builder.Services.AddSingleton<IMongoClient>(new MongoClient("mongodb://localhost:27017"));
        builder.Services.AddSingleton(provider => provider.GetRequiredService<IMongoClient>().GetDatabase("Quickstart"));
        builder.Services.AddTransient(provider => provider.GetRequiredService<IMongoDatabase>().GetCollection<User>("book"));
        #endregion Snippet:Quickstart-AspNetCore-MongoSetup

        builder.Services.AddTransient(provider => provider.GetRequiredService<IMongoDatabase>().GetCollection<User>("borrowedBook"));
        builder.Services.AddTransient(provider => provider.GetRequiredService<IMongoDatabase>().GetCollection<User>("overdueBook"));
        builder.Services.AddTransient(provider => provider.GetRequiredService<IMongoDatabase>().GetCollection<User>("reservedBook"));
        builder.Services.AddTransient(provider => provider.GetRequiredService<IMongoDatabase>().GetCollection<User>("users"));
    }
}
