// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Shop.Carts.Starting;

namespace Shop.Specs.Carts.Starting.for_StartCart;

public class when_starting_a_cart : Specification
{
    readonly CommandScenario<StartCart> _scenario = new();
    readonly CartId _cartId = new(Guid.Parse("aaaaaaaa-bbbb-4ccc-8ddd-eeeeeeeeeeee"));
    CommandResult _result = null!;

    void Establish() => _scenario.PinGeneratedValue("cart", _cartId);
    async Task Because() => _result = await _scenario.Execute(new StartCart());
    void Destroy() => _scenario.Dispose();

    [Fact] void should_succeed() => _result.ShouldBeSuccessful();
    [Fact] void should_return_the_pinned_cart_id() => ((CommandResult<CartId>)_result).Response.ShouldEqual(_cartId);
}
