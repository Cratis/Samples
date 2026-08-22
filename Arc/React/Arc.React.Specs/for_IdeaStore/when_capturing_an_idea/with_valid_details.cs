// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Arc.React.Specs.for_IdeaStore.when_capturing_an_idea;

using Arc.React.Ideas.Board;
using Cratis.Specifications;
using Xunit;

public class with_valid_details : Specification
{
    readonly IdeaStore _store = new();
    readonly IdeaId _ideaId = IdeaId.New();

    void Because() => new CaptureIdea(_ideaId, "Make setup visible", "Show the shortest path from clone to a running slice.").Handle(_store);
    void Destroy() => _store.Dispose();

    [Fact] void should_add_one_idea() => _store.Current.Count.ShouldEqual(1);
    [Fact] void should_keep_the_assigned_identifier() => _store.Current.Single().Id.ShouldEqual(_ideaId);
    [Fact] void should_keep_the_title() => _store.Current.Single().Title.ShouldEqual((IdeaTitle)"Make setup visible");
}
