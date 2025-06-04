// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;
using MongoDB.Driver;

namespace Library.given;

public class a_client(ChronicleOutOfProcessFixture fixture) : SpecificationContext(fixture)
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    protected HttpClient Client { get; private set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    void Establish()
    {
        Client = CreateClient(new()
        {
            BaseAddress = new("http://localhost:8080")
        });
    }
}
