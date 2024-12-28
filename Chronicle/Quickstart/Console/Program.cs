#region Snippet:Quickstart-Console-Setup
using Cratis.Chronicle;

using var client = new ChronicleClient("chronicle://localhost:35000");
var eventStore = client.GetEventStore("Quickstart");
await eventStore.DiscoverAll();
await eventStore.RegisterAll();
#endregion Snippet:Quickstart-Console-Setup
