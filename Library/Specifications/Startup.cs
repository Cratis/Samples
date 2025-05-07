// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace eCommerce.Specs;

public class Startup
{
    public void Configure(IApplicationBuilder app)
    {
        app.UseCratisChronicle();
        app.UseCratisApplicationModel();
        app.UseEndpoints(_ => _.MapControllers());
    }
}
