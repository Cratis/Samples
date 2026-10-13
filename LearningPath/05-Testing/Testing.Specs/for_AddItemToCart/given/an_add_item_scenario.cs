// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace LearningPath.Testing.Carts.for_AddItemToCart.given;

public class an_add_item_scenario : Specification
{
    protected readonly CartId _cartId = CartId.New();
    protected CommandScenario<AddItemToCart> _scenario;
    protected CommandResult _result;

    void Establish() => _scenario = new();
    void Destroy() => _scenario.Dispose();
}
