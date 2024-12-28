using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
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

BsonSerializer
    .RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

var pack = new ConventionPack
{
    // We want to ignore extra elements that might be in the documents, Chronicle adds some metadata to the documents
    new IgnoreExtraElementsConvention(true),

    // Chronicle uses camelCase for element names, so we need to use this convention
    new CamelCaseElementNameConvention()
};
ConventionRegistry.Register("conventions", pack, t => true);

Globals.Books = new Quickstart.Common.Books(Globals.Database.GetCollection<Book>("books"));
Globals.BorrowedBooks = new Quickstart.Common.BorrowedBooks(Globals.Database.GetCollection<BorrowedBook>("borrowedBooks"));
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
