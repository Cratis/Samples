// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;
using eCommerce.Specs;
using MongoDB.Driver;

namespace eCommerce.given.Specs;

public class a_client(GlobalFixture globalFixture) : IntegrationSpecificationContext(globalFixture)
{
    protected HttpClient HttpClient;
    readonly GlobalFixture _globalFixture = globalFixture;

    protected async Task<TModel> GetModel<TModel>(EventSourceId eventSourceId)
    {
        var isGuid = Guid.TryParse(eventSourceId.ToString(), out var guid);
        var filter = isGuid ?
            Builders<TModel>.Filter.Eq(new StringFieldDefinition<TModel, Guid>("_id"), guid) :
            Builders<TModel>.Filter.Eq(new StringFieldDefinition<TModel, string>("_id"), eventSourceId);

        var result = await _globalFixture.ReadModels.Database.GetCollection<TModel>().FindAsync(filter);
        return result.FirstOrDefault();
    }

    void Establish()
    {
        HttpClient = CreateClient(new()
        {
            BaseAddress = new Uri("http://localhost:5000/")
        });
    }
}
