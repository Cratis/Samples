// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;

namespace Shop.GeneratedPolicies;

/// <summary>
/// Registers the policies generated from the Screenplay application.
/// </summary>
public static partial class Registration
{
    /// <summary>
    /// Registers generated policies when the application contains protected operations.
    /// </summary>
    /// <param name="services">The application's services.</param>
    public static void Register(IServiceCollection services) => RegisterGenerated(services);

    static partial void RegisterGenerated(IServiceCollection services);
}
