// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;
using Cratis.Chronicle.EventSequences;

namespace Chronicle.Backend;

sealed class Timeline(IEventLog eventLog)
{
    public async Task<AppendResult> Record(TimelineId timelineId, string text) =>
        await eventLog.Append(timelineId, new TimelineEntryRecorded(text));

    public async Task<IReadOnlyList<TimelineHistoryEntry>> GetHistory(TimelineId timelineId)
    {
        var events = await eventLog.GetForEventSourceIdAndEventTypes(
            timelineId,
            [typeof(TimelineEntryRecorded).GetEventType()]);

        return
        [
            .. events.Select(appendedEvent => new TimelineHistoryEntry(
                appendedEvent.Context.SequenceNumber.Value,
                appendedEvent.Context.Occurred,
                ((TimelineEntryRecorded)appendedEvent.Content).Text))
        ];
    }
}

sealed record TimelineHistoryEntry(ulong SequenceNumber, DateTimeOffset Occurred, string Text);
