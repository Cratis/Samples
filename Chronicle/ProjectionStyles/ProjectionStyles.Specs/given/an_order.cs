// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace ProjectionStyles.Specs.given;

public class an_order<TOrder> : Specification
    where TOrder : class
{
    protected readonly OrderId _orderId = OrderId.New();
    protected readonly ProductId _productId = ProductId.New();
    protected readonly ProductId _otherProductId = ProductId.New();
    protected ReadModelScenario<TOrder> _scenario;
    protected TOrder _order => _scenario.Instance!;

    void Establish() => _scenario = new ReadModelScenario<TOrder>();
    void Destroy() => _scenario.Dispose();
}
