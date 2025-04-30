// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;
using Cratis.Chronicle.EventSequences;

namespace eCommerce.Specs;

public static class EventForEventSourceIdHelpers
{
    public static EventForEventSourceId Create(object content, EventSourceId? eventSourceId = null, Cratis.Chronicle.Auditing.Causation? causation = null)
    {
        return new EventForEventSourceId(
            eventSourceId ?? $"Random event source {Random.Shared.Next()}",
            content,
            causation ?? new Cratis.Chronicle.Auditing.Causation(DateTimeOffset.UtcNow, Cratis.Chronicle.Auditing.CausationType.Unknown, new Dictionary<string, string>()));
    }

    public static IEnumerable<EventForEventSourceId> CreateMultiple(Func<int, object> content, int num, EventSourceId? eventSourceId = null)
        => Enumerable.Range(0, num).Select(i => Create(content(i), eventSourceId));
}
