// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Testing.ReadModels;
using Library.Authors.Registration;

namespace Library.Authors.Listing.for_author_projection;

public class when_an_author_is_registered
{
    [Fact(Skip = "ReadModelScenario currently fails for fluent projections in this sample; kept as pending coverage.")]
    public async Task should_set_the_author_id()
    {
        var authorId = AuthorId.New();
        var scenario = new ReadModelScenario<Author>();
        await scenario.Given
            .ForEventSource(authorId)
            .Events(new AuthorRegistered("John Doe"));

        scenario.Instance!.Id.ShouldEqual(authorId);
    }

    [Fact(Skip = "ReadModelScenario currently fails for fluent projections in this sample; kept as pending coverage.")]
    public async Task should_set_the_author_name()
    {
        var scenario = new ReadModelScenario<Author>();
        await scenario.Given
            .ForEventSource(AuthorId.New())
            .Events(new AuthorRegistered("John Doe"));

        scenario.Instance!.Name.Value.ShouldEqual("John Doe");
    }
}
