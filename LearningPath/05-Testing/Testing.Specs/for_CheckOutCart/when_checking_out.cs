// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace LearningPath.Testing.Carts.for_CheckOutCart;

public class when_checking_out : Specification
{
    readonly CartId _cartId = CartId.New();
    readonly CommandScenario<CheckOutCart> _scenario = new();
    CommandResult _result;

    async Task Because() => _result = await _scenario.Execute(new CheckOutCart(_cartId));
    void Destroy() => _scenario.Dispose();

    [Fact] void should_succeed() => _result.ShouldBeSuccessful();
    [Fact] async Task should_append_checkout_to_the_cart() => await _scenario.ShouldHaveAppendedEvent<CheckOutCart, CartCheckedOut>(_cartId);
}
