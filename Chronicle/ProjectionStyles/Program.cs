// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle;
using Cratis.Chronicle.Events;
using Cratis.Chronicle.Observation;
using ProjectionStyles;

var builder = WebApplication.CreateBuilder(args)
    .AddCratisChronicle(options => options.EventStore = "ProjectionStylesSample");

var app = builder.Build();
app.UseCratisChronicle();

app.MapGet("/", () => Results.Ok(new { Sample = "Projection styles", Run = "POST /orders/run" }));

app.MapPost("/orders/run", async (IEventStore eventStore) =>
{
    // Register a separate schema for the PDL preview, not a fourth code-owned projection.
    await eventStore.ReadModels.Register<PdlOrder>();
    var productId = ProductId.New();
    var productResult = await eventStore.EventLog.AppendMany(productId, [new ProductRegistered("Trail mug"), new ProductRenamed("Camp mug")]);
    if (!productResult.IsSuccess)
    {
        return Results.Problem("Chronicle rejected the product events.");
    }

    var productCompletion = await productResult.WaitForCompletion(TimeSpan.FromSeconds(10));
    if (!productCompletion.IsSuccess)
    {
        return Results.Problem("Chronicle observers failed while processing the product events.");
    }

    var orderId = OrderId.New();
    var orderResult = await eventStore.EventLog.AppendMany(
        orderId,
        [
            new OrderPlaced("Sample customer"),
            new ItemAddedToOrder(productId, 2),
            new ItemAddedToOrder(productId, 5),
            new ItemQuantityAdjusted(productId, 3),
            new OrderShipped()
        ]);
    if (!orderResult.IsSuccess)
    {
        return Results.Problem("Chronicle rejected the order events.");
    }

    var orderCompletion = await orderResult.WaitForCompletion(TimeSpan.FromSeconds(10));
    if (!orderCompletion.IsSuccess)
    {
        return Results.Problem("Chronicle observers failed while processing the order events.");
    }

    var modelBound = await eventStore.ReadModels.GetInstanceById<ModelBoundOrder>((EventSourceId)orderId);
    var fluent = await eventStore.ReadModels.GetInstanceById<FluentOrder>((EventSourceId)orderId);
    var reduced = await eventStore.ReadModels.GetInstanceById<ReducedOrder>((EventSourceId)orderId);
    if (modelBound is null || fluent is null || reduced is null)
    {
        return Results.Problem("One or more order read models were not materialized.");
    }

    return Results.Ok(new { OrderId = orderId.Value, ProductId = productId.Value, ModelBound = modelBound, Fluent = fluent, Reduced = reduced });
});

await app.RunAsync();
