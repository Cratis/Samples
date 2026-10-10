// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle;
using Cratis.Chronicle.Connections;
using Cratis.Chronicle.Events;
using Cratis.Chronicle.Observation;
using LearningPath.FirstProjection;

// Connecting also registers the projection declared on CartSummary with Chronicle.
using var client = new ChronicleClient(ChronicleConnectionString.Development);
var eventStore = await client.GetEventStore("LearningPathFirstProjection");

var cartId = CartId.New();

var appendResult = await eventStore.EventLog.AppendMany(
    cartId,
    [
        new ItemAddedToCart("Coffee beans", 2),
        new ItemAddedToCart("Oat milk", 1),
        new ItemRemovedFromCart("Oat milk", 1),
        new ItemAddedToCart("Cinnamon buns", 4)
    ]);

if (!appendResult.IsSuccess)
{
    Console.WriteLine("Chronicle rejected the events. Check that the Chronicle server is running.");
    return 1;
}

// Projections run in the background. Wait for them to catch up before reading our own write.
var completion = await appendResult.WaitForCompletion(TimeSpan.FromSeconds(10));
if (!completion.IsSuccess)
{
    Console.WriteLine("The projection did not finish processing the events.");
    return 1;
}

var cart = await eventStore.ReadModels.GetInstanceById<CartSummary>((EventSourceId)cartId);
Console.WriteLine($"Cart {cartId.Value} holds {cart.ItemCount} items. Checked out: {cart.CheckedOut}.");

// One more fact, and the read model follows.
var checkoutResult = await eventStore.EventLog.Append(cartId, new CartCheckedOut());
if (!checkoutResult.IsSuccess || !(await checkoutResult.WaitForCompletion(TimeSpan.FromSeconds(10))).IsSuccess)
{
    Console.WriteLine("The checkout was not projected.");
    return 1;
}

cart = await eventStore.ReadModels.GetInstanceById<CartSummary>((EventSourceId)cartId);
Console.WriteLine($"Cart {cartId.Value} holds {cart.ItemCount} items. Checked out: {cart.CheckedOut}.");

return 0;
