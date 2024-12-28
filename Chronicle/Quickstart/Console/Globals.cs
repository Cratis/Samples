using Cratis.Chronicle;
using MongoDB.Driver;

namespace Quickstart;

internal static class Globals
{
    internal static IEventStore EventStore { get; set; } = default!;
    internal static IMongoDatabase Database { get; set; } = default!;
    internal static Common.Books Books { get; set; } = default!;
    internal static Common.BorrowedBooks BorrowedBooks { get; set; } = default!;
    internal static Users Users { get; set; } = default!;
    internal static DemoData DemoData { get; set; } = default!;
}
