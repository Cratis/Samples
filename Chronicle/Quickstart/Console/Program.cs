using System;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Quickstart;
using Terminal.Gui;

#region Snippet:Quickstart-Console-Setup
using Cratis.Chronicle;

using var client = new ChronicleClient("chronicle://localhost:35000");
var eventStore = client.GetEventStore("Quickstart");
await eventStore.DiscoverAll();
await eventStore.RegisterAll();
#endregion Snippet:Quickstart-Console-Setup

Globals.EventStore = eventStore;
var mongoClient = new MongoClient("mongodb://localhost:27017");
Globals.Database = mongoClient.GetDatabase("Quickstart");
var pack = new ConventionPack
{
    new IgnoreExtraElementsConvention(true),
    new CamelCaseElementNameConvention()
};
ConventionRegistry.Register("conventions", pack, t => true);

Globals.Books = new Quickstart.Common.Books(Globals.Database.GetCollection<Book>("books"));

Application.Init();

try
{
    Application.Run(new Library());
}
finally
{
    Application.Shutdown();
}
