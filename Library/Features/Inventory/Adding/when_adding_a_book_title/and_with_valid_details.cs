// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Commands;
using Cratis.Chronicle.XUnit.Integration.Events;
using Library.Authors.Registration;
using context = Library.Inventory.Adding.when_adding_a_book_title.and_with_valid_details.context;

namespace Library.Inventory.Adding.when_adding_a_book_title;

[Collection(ChronicleCollection.Name)]
public class and_with_valid_details(context context) : Given<context>(context)
{
    public class context(ChronicleOutOfProcessFixture fixture) : given.an_http_client(fixture)
    {
        public const string Title = "The Last Wish";
        public static readonly ISBN Isbn = "978-0316389704";
        public static readonly AuthorId AddedByAuthorId = AuthorId.New();
        public CommandResult<BookAddedToInventory> Result;

        Task Establish() => EventStore.EventLog.Append(AddedByAuthorId, new AuthorRegistered("Existing Author"));

        async Task Because()
        {
            Result = await Client.ExecuteCommand<AddBookTitleToInventory, BookAddedToInventory>(
                "/api/inventory/adding",
                new(Title, Isbn, AddedByAuthorId, 2));
        }
    }

    [Fact] void should_be_successful() => Context.Result.IsSuccess.ShouldBeTrue();

    [Fact] void should_return_the_event_payload() => Context.Result.Response.Title.Value.ShouldEqual(context.Title);

    [Fact] void should_append_a_single_event() => Context.ShouldHaveTailSequenceNumber(EventSequenceNumber.First);
}
