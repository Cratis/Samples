// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Aggregates;
using eCommerce.given.Specs;

namespace eCommerce.Specs.Features.Carts.given;

public class an_empty_cart(GlobalFixture globalFixture) : a_client(globalFixture)
{
    AggregateRootCommitResult commitResult;

    void Establish()
    {
    }
}