// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Read.Books;

namespace Api.Books;

[Route("/api/books/catalog")]
public class Catalog : ControllerBase
{
    [HttpGet]
    public Task<IEnumerable<Book>> AllBooks()
    {
        return Task.FromResult(Enumerable.Empty<Book>());
    }

    public Task RegisterBook([FromBody] RegisterBook command)
    {
        return Task.CompletedTask;
    }
}
