// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using eCommerce.Specs;

namespace eCommerce.given.Specs;

public class a_client(GlobalFixture globalFixture) : IntegrationSpecificationContext(globalFixture)
{
    protected HttpClient HttpClient;

    void Establish()
    {
        HttpClient = CreateClient(new()
        {
            BaseAddress = new Uri("http://localhost:5000/")
        });
    }
}