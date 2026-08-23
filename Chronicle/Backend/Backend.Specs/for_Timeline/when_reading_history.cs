// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Chronicle.Backend.Specs.for_Timeline;

public class when_reading_history : Specification
{
    const string FirstEntry = "The first fact.";
    const string SecondEntry = "The next fact.";
    IEventLog _eventLog;
    Timeline _timeline;
    TimelineId _timelineId;
    IReadOnlyList<TimelineHistoryEntry> _history;

    void Establish()
    {
        _eventLog = Substitute.For<IEventLog>();
        _eventLog
            .GetForEventSourceIdAndEventTypes(
                Arg.Any<EventSourceId>(),
                Arg.Any<IEnumerable<EventType>>())
            .Returns(Task.FromResult<IImmutableList<AppendedEvent>>(
                ImmutableList.Create(
                    AppendedEvent.EmptyWithContent(new TimelineEntryRecorded(FirstEntry)),
                    AppendedEvent.EmptyWithContent(new TimelineEntryRecorded(SecondEntry)))));
        _timeline = new(_eventLog);
        _timelineId = TimelineId.New();
    }

    async Task Because() => _history = await _timeline.GetHistory(_timelineId);

    [Fact] void should_return_the_recorded_entries() => _history.Select(_ => _.Text).ShouldContainOnly([FirstEntry, SecondEntry]);
    [Fact]
    async Task should_query_with_the_typed_event_source_identifier() => await _eventLog.Received(1).GetForEventSourceIdAndEventTypes(
        (EventSourceId)_timelineId,
        Arg.Is<IEnumerable<EventType>>(types => types.Single() == typeof(TimelineEntryRecorded).GetEventType()));
}
