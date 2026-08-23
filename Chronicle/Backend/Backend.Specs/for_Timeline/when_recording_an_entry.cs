// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Chronicle.Backend.Specs.for_Timeline;

public class when_recording_an_entry : Specification
{
    const string EntryText = "Chronicle keeps the facts.";
    IEventLog _eventLog;
    Timeline _timeline;
    TimelineId _timelineId;
    AppendResult _result;

    void Establish()
    {
        _eventLog = Substitute.For<IEventLog>();
        _eventLog
            .Append(Arg.Any<EventSourceId>(), Arg.Any<object>())
            .Returns(AppendResult.Success(CorrelationId.New(), 42));
        _timeline = new(_eventLog);
        _timelineId = TimelineId.New();
    }

    async Task Because() => _result = await _timeline.Record(_timelineId, EntryText);

    [Fact] void should_return_the_append_result() => _result.SequenceNumber.Value.ShouldEqual(42UL);
    [Fact]
    async Task should_append_the_typed_event() => await _eventLog.Received(1).Append(
        (EventSourceId)_timelineId,
        Arg.Is<TimelineEntryRecorded>(@event => @event.Text.Value == EntryText));
}
