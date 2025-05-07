// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;
using Cratis.Chronicle.XUnit.Integration;
using MongoDB.Driver;

namespace Library.given.Specs;

public class a_client(ChronicleFixture chronicleFixture) : IntegrationSpecificationContext(chronicleFixture)
{
    protected HttpClient HttpClient;
    readonly ChronicleFixture _chronicleFixture = chronicleFixture;

    protected async Task<TModel> GetModel<TModel>(EventSourceId eventSourceId)
    {
        var isGuid = Guid.TryParse(eventSourceId.ToString(), out var guid);
        var filter = isGuid ?
            Builders<TModel>.Filter.Eq(new StringFieldDefinition<TModel, Guid>("_id"), guid) :
            Builders<TModel>.Filter.Eq(new StringFieldDefinition<TModel, string>("_id"), eventSourceId);

        var result = await _chronicleFixture.ReadModels.Database.GetCollection<TModel>().FindAsync(filter);
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
