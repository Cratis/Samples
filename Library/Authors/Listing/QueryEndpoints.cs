// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Library.Authors.Listing;

[Route("/api/authors")]
public class Authors : ControllerBase
{
    [HttpGet]
    public Task<IEnumerable<Author>> AllAuthors()
    {
        return Task.FromResult(Enumerable.Empty<Author>());
    }
}
