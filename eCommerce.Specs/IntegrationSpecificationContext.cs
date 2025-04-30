// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

namespace eCommerce.Specs;

public abstract class IntegrationSpecificationContext(GlobalFixture globalFixture) : OrleansFixture(globalFixture), IAsyncLifetime
{
    public async Task InitializeAsync()
    {
        EnsureBuilt();

        // TODO: This is a workaround for the fact that the test host is not fully initialized when the test starts.
        await Task.Delay(TimeSpan.FromSeconds(1));
        await OnEstablish();
        await OnBecause();
    }

    public new Task DisposeAsync() => OnDestroy();

    Task OnEstablish()
    {
        return InvokeMethod("Establish");
    }

    Task OnBecause()
    {
        return InvokeMethod("Because");
    }

    Task OnDestroy()
    {
        return InvokeMethod("Destroy");
    }

    Task InvokeMethod(string name)
    {
        return typeof(SpecificationMethods<,>).MakeGenericType(GetType(), typeof(Specification)).GetMethod(name, BindingFlags.Static | BindingFlags.Public).Invoke(null, [this]) as Task;
    }
}
