// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Library.Lenders.Registration;

namespace Library.Lenders.Listing;

public class LenderProjection : IProjectionFor<Lender>
{
    public void Define(IProjectionBuilderFor<Lender> builder) => builder
        .AutoMap()
        .From<LenderRegistered>();
}
