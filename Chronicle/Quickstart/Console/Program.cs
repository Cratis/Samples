// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using MongoDB.Bson;
using MongoDB.Driver;
using Quickstart;
using Terminal.Gui;

#region Snippet:Quickstart-Console-Setup
using Cratis.Chronicle;

// Explicitly use the Chronicle Options to set the naming policy to camelCase for the projection/reducer sinks
using var client = new ChronicleClient(ChronicleOptions.FromUrl("http://localhost:35000").WithCamelCaseNamingPolicy());
var eventStore = await client.GetEventStore("Quickstart");
#endregion Snippet:Quickstart-Console-Setup

MongoDBDefaults.Initialize();

var mongoClient = new MongoClient("mongodb://localhost:27017");
Globals.Database = mongoClient.GetDatabase("Quickstart");
Globals.EventStore = eventStore;
Globals.Books = new Books(Globals.Database.GetCollection<Book>("books"));
Globals.BorrowedBooks = new BorrowedBooks(Globals.Database.GetCollection<BorrowedBook>("borrowedBooks"));
Globals.OverdueBooks = new OverdueBooks(Globals.Database.GetCollection<OverdueBook>("overdueBooks"));
Globals.ReservedBooks = new ReservedBooks(Globals.Database.GetCollection<ReservedBook>("reservedBooks"));
Globals.Users = new Users(Globals.Database.GetCollection<User>("users"));
Globals.DemoData = new DemoData(Globals.EventStore.EventLog);

Application.Init();
try
{
    Application.Run(new Library());
}
finally
{
    Application.Shutdown();
}
