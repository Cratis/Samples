// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle;
using Cratis.Chronicle.Events;
using Cratis.Chronicle.EventSequences;
using CrossStore;

var builder = WebApplication.CreateBuilder(args)
    .AddCratisChronicle(options => options.EventStore = StoreNames.Fulfillment);

var app = builder.Build();
app.UseCratisChronicle();

var chronicle = app.Services.GetRequiredService<IChronicleClient>();
await chronicle.GetEventStore(StoreNames.Orders);
var fulfillmentStore = await chronicle.GetEventStore(StoreNames.Fulfillment);
await fulfillmentStore.Subscriptions.Subscribe(
    StoreSubscriptionIds.OrdersForFulfillment,
    StoreNames.Orders,
    subscription => subscription.WithEventType<OrderRequestedForFulfillment>());

app.MapGet("/", () => Results.Ok(new
{
    Sample = "Chronicle cross-store outbox/inbox",
    SourceEventStore = StoreNames.Orders,
    TargetEventStore = StoreNames.Fulfillment,
    Subscription = StoreSubscriptionIds.OrdersForFulfillment,
    PlaceOrder = "POST /orders/{orderId}",
    FulfillmentOrder = "GET /fulfillment/orders/{orderId}"
}));

app.MapPost("/orders/{orderId:guid}", async (Guid orderId, PlaceOrderRequest request, IChronicleClient client) =>
{
    if (string.IsNullOrWhiteSpace(request.Buyer) || string.IsNullOrWhiteSpace(request.Sku) || request.Quantity <= 0)
    {
        return Results.BadRequest(new { Error = "Buyer and sku are required, and quantity must be greater than zero." });
    }

    OrderId typedOrderId = orderId;
    BuyerReference buyer = request.Buyer.Trim();
    ProductSku sku = request.Sku.Trim();
    OrderQuantity quantity = request.Quantity;
    var ordersStore = await client.GetEventStore(StoreNames.Orders);

    var orderAppend = await ordersStore.EventLog.Append(typedOrderId, new OrderPlaced(buyer, sku, quantity));
    if (!orderAppend.IsSuccess)
    {
        return Results.Problem("Chronicle rejected the source-owned order fact.");
    }

    var contractAppend = await ordersStore
        .GetEventSequence(EventSequenceId.Outbox)
        .Append(typedOrderId, new OrderRequestedForFulfillment(sku, quantity));

    if (!contractAppend.IsSuccess)
    {
        return Results.Problem(
            "The order was recorded, but publishing its fulfillment contract failed. The source fact was not rolled back.",
            statusCode: StatusCodes.Status503ServiceUnavailable);
    }

    return Results.Accepted(
        $"/fulfillment/orders/{orderId}",
        new PlaceOrderAccepted(orderId, StoreNames.Orders, StoreNames.Fulfillment));
});

app.MapGet("/fulfillment/orders/{orderId:guid}", async (Guid orderId, IChronicleClient client) =>
{
    var targetStore = await client.GetEventStore(StoreNames.Fulfillment);
    FulfillmentOrderId fulfillmentOrderId = orderId;
    var order = await targetStore.ReadModels.GetInstanceById<FulfillmentOrder>((EventSourceId)fulfillmentOrderId);

    return order is null ? Results.NotFound() : Results.Ok(order);
});

await app.RunAsync();
