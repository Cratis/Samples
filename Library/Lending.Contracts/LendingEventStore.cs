// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

[assembly: EventStore(Lending.Contracts.LendingEventStore.Name)]

namespace Lending.Contracts;

/// <summary>
/// The event store the Lending service runs in, and therefore the store its contract events are published from.
/// </summary>
/// <remarks>
/// The Lending service connects with this name and the contract assembly declares it as the source store,
/// so consumers such as Members subscribe to the outbox the events are actually appended to.
/// </remarks>
public static class LendingEventStore
{
    /// <summary>
    /// The name of the event store.
    /// </summary>
    public const string Name = "Library";
}
