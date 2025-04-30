// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;
using Cratis.Json;
using eCommerce.Carts.AddItem;
using eCommerce.Carts.Contents;
using context = eCommerce.Specs.Features.Carts.when_adding_item_to_empty_cart.context;

namespace eCommerce.Specs.Features.Carts;

[Collection(GlobalCollection.Name)]
public class when_adding_item_to_empty_cart(context context) : Given<context>(context)
{
    public class context(GlobalFixture globalFixture) : given.an_empty_cart(globalFixture)
    {
        public HttpResponseMessage Response { get; private set; }
        public Cart ReadModel { get; private set; }

        async Task Because()
        {
            Response = await HttpClient.PostAsJsonAsync($"/api/carts/{Guid.Empty}/items", new AddItemToCart("123"), Globals.JsonSerializerOptions);

            var observer = GetObserverForProjection<CartProjection>();
            await observer.WaitTillReachesEventSequenceNumber(EventSequenceNumber.First);
            ReadModel = await GetModel<Cart>(Guid.Empty);
        }
    }

    [Fact] void should_not_fail() => Context.Response.IsSuccessStatusCode.ShouldBeTrue();
    [Fact] void should_have() => Context.EventStore.EventLog.GetFromSequenceNumber(EventSequenceNumber.First).Result.Count.ShouldEqual(1);
    [Fact] void should_one_item_in_cart() => Context.ReadModel.Items.Count().ShouldEqual(1);
}
