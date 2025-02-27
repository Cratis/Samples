// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle;
using MongoDB.Driver;

namespace Quickstart;

internal static class Globals
{
    internal static IEventStore EventStore { get; set; } = default!;
    internal static IMongoDatabase Database { get; set; } = default!;
    internal static DemoData DemoData { get; set; } = default!;
    internal static Books Books { get; set; } = default!;
    internal static BorrowedBooks BorrowedBooks { get; set; } = default!;
    internal static OverdueBooks OverdueBooks { get; set; } = default!;
    internal static ReservedBooks ReservedBooks { get; set; } = default!;
    internal static Users Users { get; set; } = default!;
}
