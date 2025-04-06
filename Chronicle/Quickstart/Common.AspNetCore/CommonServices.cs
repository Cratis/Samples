// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Quickstart.Common.AspNetCore;

public static class CommonServices
{
    public static void AddArtifacts(this WebApplicationBuilder builder)
    {
        #region Snippet:Quickstart-AspNetCore-ArtifactsServices
        builder.Services.AddTransient<UsersReactor>();
        builder.Services.AddTransient<BooksReducer>();
        builder.Services.AddTransient<BorrowedBooksProjection>();
        builder.Services.AddTransient<OverdueBooksProjection>();
        builder.Services.AddTransient<ReservedBooksProjection>();
        #endregion Snippet:Quickstart-AspNetCore-ArtifactsServices

        #region Snippet:Quickstart-AspNetCore-Services
        builder.Services.AddTransient<Books>();
        builder.Services.AddTransient<BorrowedBooks>();
        builder.Services.AddTransient<OverdueBooks>();
        builder.Services.AddTransient<ReservedBooks>();
        builder.Services.AddTransient<Users>();
        builder.Services.AddTransient<DemoData>();
        #endregion Snippet:Quickstart-AspNetCore-Services
    }
}
