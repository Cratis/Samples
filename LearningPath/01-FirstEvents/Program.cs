// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle;
using Cratis.Chronicle.Connections;
using Cratis.Chronicle.Events;
using LearningPath.FirstEvents;

// Connect to the local Chronicle development server and open this step's event store.
using var client = new ChronicleClient(ChronicleConnectionString.Development);
var eventStore = await client.GetEventStore("LearningPathFirstEvents");

// Every run starts a new cart, so every run appends to a fresh event source.
var cartId = CartId.New();

var appendResult = await eventStore.EventLog.AppendMany(
    cartId,
    [
        new ItemAddedToCart("Coffee beans", 2),
        new ItemAddedToCart("Oat milk", 1),
        new ItemRemovedFromCart("Oat milk", 1),
        new CartCheckedOut()
    ]);

if (!appendResult.IsSuccess)
{
    Console.WriteLine("Chronicle rejected the events. Check that the Chronicle server is running.");
    return 1;
}

Console.WriteLine($"Appended 4 events for cart {cartId.Value}.");
Console.WriteLine();

// Read the cart's history back, in the order the events were appended.
var history = await eventStore.EventLog.GetFromSequenceNumber(EventSequenceNumber.First, cartId);

foreach (var appendedEvent in history)
{
    Console.WriteLine($"#{appendedEvent.Context.SequenceNumber.Value} at {appendedEvent.Context.Occurred:HH:mm:ss}: {appendedEvent.Content}");
}

return 0;
