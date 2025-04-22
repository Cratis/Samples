// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.EventSequences;
using Cratis.Execution;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

namespace Quickstart.Common.AspNetCore;

public static class Api
{
    public static void MapApi(this WebApplication app)
    {
        // Add a middleware that sets a new correlation ID for each request
        app.Use(async (context, next) =>
        {
            CorrelationIdAccessor.SetCurrent(Guid.NewGuid());
            await next.Invoke();
        });

        app.MapGet("/api/demo-data", ([FromServices] DemoData demoData) => demoData.Initialize());
        app.MapPost("/api/books/reserve", async ([FromServices] IEventLog eventLog) =>
            await eventLog.Append(Guid.NewGuid(), new BookReservationPlaced(Guid.NewGuid())));

        #region Snippet:Quickstart-AspNetCore-BookBorrowed
        app.MapPost("/api/books/{bookId}/borrow/{userId}", async (
            [FromServices] IEventLog eventLog,
            [FromRoute] Guid bookId,
            [FromRoute] Guid userId) => await eventLog.Append(bookId, new BookBorrowed(userId)));
        #endregion Snippet:Quickstart-AspNetCore-BookBorrowed
    }
}
