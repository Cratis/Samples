// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Applications.Commands;
using Cratis.Chronicle.Events;
using Cratis.Chronicle.XUnit.Integration.Events;
using context = Library.Authors.Registration.when_registering.and_there_are_no_authors.context;

namespace Library.Authors.Registration.when_registering;

[Collection(ChronicleCollection.Name)]
public class and_there_are_no_authors(context context) : Given<context>(context)
{
    public class context(ChronicleOutOfProcessFixture fixture) : given.a_client(fixture)
    {
        public const string AuthorName = "John Doe";
        public CommandResult<Guid> Result;

        async Task Because()
        {
            Result = await Client.ExecuteCommand<RegisterAuthor, Guid>("/api/authors/register", new RegisterAuthor(AuthorName));
        }
    }

    [Fact]
    void should_be_successful() => Context.Result.IsSuccess.ShouldBeTrue();

    [Fact] void should_have_appended_only_one_event() => Context.ShouldHaveTailSequenceNumber(EventSequenceNumber.First);

    [Fact] void should_append_1_author_registered_event() => Context.ShouldHaveAppendedEvent<AuthorRegistered>(EventSequenceNumber.First, Context.Result.Response, evt => evt.Name.Value.ShouldEqual(context.AuthorName));
}
