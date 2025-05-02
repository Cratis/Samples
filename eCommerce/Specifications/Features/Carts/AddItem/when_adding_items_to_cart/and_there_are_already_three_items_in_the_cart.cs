// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Grains.Observation;
using Cratis.Json;
using eCommerce.Specs;
using context = eCommerce.Carts.AddItem.when_adding_items_to_cart.and_there_are_already_three_items_in_the_cart.context;

namespace eCommerce.Carts.AddItem.when_adding_items_to_cart;

[Collection(GlobalCollection.Name)]
public class and_there_are_already_three_items_in_the_cart(context context) : Given<context>(context)
{
    public class context(GlobalFixture globalFixture) : given.an_empty_cart(globalFixture)
    {
        public HttpResponseMessage Response { get; private set; }

        async Task Establish()
        {
            await EventStore.EventLog.Append(Guid.Empty, new ItemAddedToCart("something", 42));
            await EventStore.EventLog.Append(Guid.Empty, new ItemAddedToCart("something", 42));
            await EventStore.EventLog.Append(Guid.Empty, new ItemAddedToCart("something", 42));
        }

        async Task Because()
        {
            Response = await HttpClient.PostAsJsonAsync($"/api/carts/{Guid.Empty}/items", new AddItemToCart(Guid.Empty, "something", 42), Globals.JsonSerializerOptions);
        }
    }

    [Fact] void should_fail() => Context.Response.IsSuccessStatusCode.ShouldBeFalse();
 }
