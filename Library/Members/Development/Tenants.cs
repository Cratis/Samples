// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
using Cratis.Arc.Tenancy;

namespace Members.Development;

public class Tenants : ICanProvideTenants
{
    public Task<IEnumerable<Tenant>> Provide() => Task.FromResult<IEnumerable<Tenant>>(
    [
        new Tenant("central", "Central Library"),
        new Tenant("westside", "Westside Branch"),
    ]);
}
#endif
