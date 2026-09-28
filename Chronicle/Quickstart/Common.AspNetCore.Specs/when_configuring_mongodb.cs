// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Quickstart.Common;
using Quickstart.Common.AspNetCore;
using Xunit;

namespace Quickstart.Common.AspNetCore.Specs;

public class when_configuring_mongodb
{
    [Fact]
    public void should_use_chronicles_read_model_collection_names()
    {
        var builder = WebApplication.CreateBuilder();
        builder.AddMongoDBServices();
        using var provider = builder.Services.BuildServiceProvider();

        Assert.Equal("Books", provider.GetRequiredService<IMongoCollection<Book>>().CollectionNamespace.CollectionName);
        Assert.Equal("BorrowedBooks", provider.GetRequiredService<IMongoCollection<BorrowedBook>>().CollectionNamespace.CollectionName);
        Assert.Equal("OverdueBooks", provider.GetRequiredService<IMongoCollection<OverdueBook>>().CollectionNamespace.CollectionName);
        Assert.Equal("ReservedBooks", provider.GetRequiredService<IMongoCollection<ReservedBook>>().CollectionNamespace.CollectionName);
        Assert.Equal("Users", provider.GetRequiredService<IMongoCollection<User>>().CollectionNamespace.CollectionName);
    }

    [Fact]
    public void should_keep_read_model_property_names()
    {
        MongoDBDefaults.Initialize();

        Assert.Equal(nameof(Book.Title), BsonClassMap.LookupClassMap(typeof(Book)).GetMemberMap(nameof(Book.Title)).ElementName);
    }
}
