// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Applications.Commands;
using Cratis.Chronicle.Grains.Observation;
using Cratis.Json;
using context = eCommerce.Carts.AddItem.when_adding_items_to_cart.and_there_are_already_three_items_in_the_cart.context;

namespace eCommerce.Carts.AddItem.when_adding_items_to_cart;

[Collection(ChronicleCollection.Name)]
public class and_there_are_already_three_items_in_the_cart(context context) : Given<context>(context)
{
    public class context(ChronicleOutOfProcessFixture fixture) : given.an_empty_cart(fixture)
    {
        public CommandResult<Guid> Result;

        async Task Establish()
        {
            await EventStore.EventLog.Append(Guid.Empty, new ItemAddedToCart("something", 42));
            await EventStore.EventLog.Append(Guid.Empty, new ItemAddedToCart("something", 42));
            await EventStore.EventLog.Append(Guid.Empty, new ItemAddedToCart("something", 42));
        }

        async Task Because()
        {
            Result = await Client.ExecuteCommand<AddItemToCart, Guid>($"/api/carts/{Guid.Empty}/items", new AddItemToCart(Guid.Empty, "something", 42));
        }
    }

    [Fact] void should_not_be_successful() => Context.Result.IsSuccess.ShouldBeFalse();
 }
