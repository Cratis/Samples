// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Library.Authors.Registration;

[Route("/api/books/catalog")]
public class CommandEndpoints : ControllerBase
{
    [HttpPost("register")]
    public Task RegisterAuthor([FromBody] RegisterAuthor command)
    {
        return Task.CompletedTask;
    }
}
