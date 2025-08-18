// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.XUnit.Integration;
using Microsoft.AspNetCore.TestHost;

namespace eCommerce;

#pragma warning disable CS0436

public sealed class ApiWebApplicationFactory(IChronicleSetupFixture fixture, ContentRoot contentRoot) : ChronicleWebApplicationFactory<Program>(fixture, contentRoot)
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureTestServices(services => services.Configure<ChronicleAspNetCoreOptions>(options => options.EventStore = Constants.EventStore));
    }
}
