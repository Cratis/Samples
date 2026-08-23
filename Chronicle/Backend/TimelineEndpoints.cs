// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Chronicle.Backend;

static class TimelineEndpoints
{
    public static void MapTimelineEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => Results.Ok(new SampleDescription(
            ChronicleConfiguration.EventStore,
            ChronicleConfiguration.Namespace.Value,
            "/api/timelines/{timelineId}/entries",
            "/api/timelines/{timelineId}/history")));

        var timelines = app.MapGroup("/api/timelines/{timelineId:guid}");
        timelines.MapPost("/entries", RecordEntry);
        timelines.MapGet("/history", GetHistory);
    }

    static async Task<IResult> RecordEntry(
        Guid timelineId,
        RecordTimelineEntryRequest request,
        Timeline timeline)
    {
        if (string.IsNullOrWhiteSpace(request.Text))
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                [nameof(request.Text)] = ["Text is required."]
            });
        }

        TimelineId typedTimelineId = timelineId;
        TimelineEntryText entryText = request.Text.Trim();
        var appendResult = await timeline.Record(typedTimelineId, entryText);

        if (!appendResult.IsSuccess)
        {
            return Results.Problem(
                "Inspect the application and Chronicle logs before retrying the request.",
                statusCode: StatusCodes.Status503ServiceUnavailable,
                title: "Chronicle rejected the event");
        }

        return Results.Created(
            $"/api/timelines/{timelineId:D}/history",
            new TimelineEntryAccepted(timelineId, appendResult.SequenceNumber.Value));
    }

    static async Task<IResult> GetHistory(
        Guid timelineId,
        Timeline timeline)
    {
        TimelineId typedTimelineId = timelineId;
        var history = await timeline.GetHistory(typedTimelineId);

        return Results.Ok(history);
    }
}

sealed record RecordTimelineEntryRequest(string Text);
sealed record TimelineEntryAccepted(Guid TimelineId, ulong SequenceNumber);
sealed record SampleDescription(string EventStore, string Namespace, string AppendEndpoint, string HistoryEndpoint);
