// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle;
using Cratis.Chronicle.Events;
using Cratis.Chronicle.Observation;
using Processing;

var builder = WebApplication.CreateBuilder(args)
    .AddCratisChronicle(options => options.EventStore = "ProcessingSample");

var app = builder.Build();
app.UseCratisChronicle();

app.MapGet("/", () => Results.Ok(new
{
    Sample = "Chronicle focused processing",
    Run = "POST /processing/run"
}));

app.MapPost("/processing/run", async (IEventStore eventStore) =>
{
    var workItemId = WorkItemId.New();
    var appendResult = await eventStore.EventLog.AppendMany(
        workItemId,
        [
            new WorkItemOpened("Publish the focused processing sample", 8),
            new ProgressRecorded(3),
            new ProgressRecorded(2),
            new ProgressRecorded(3),
            new WorkItemCompleted(8, 8)
        ]);

    if (!appendResult.IsSuccess)
    {
        return Results.Problem("Chronicle rejected the sample event batch.");
    }

    var completion = await appendResult.WaitForCompletion(TimeSpan.FromSeconds(10));
    if (!completion.IsSuccess)
    {
        return Results.Problem("One or more Chronicle observers failed while processing the sample batch.");
    }

    var details = await eventStore.ReadModels.GetInstanceById<WorkItemDetails>((EventSourceId)workItemId);
    var progress = await eventStore.ReadModels.GetInstanceById<WorkItemProgress>((EventSourceId)workItemId);
    var stream = await eventStore.EventLog.GetFromSequenceNumber(EventSequenceNumber.First, workItemId);
    var summary = stream.Select(_ => _.Content).OfType<CompletionSummarized>().SingleOrDefault();

    if (summary is null)
    {
        return Results.Problem("The completion reactor did not produce its summary event.");
    }

    return Results.Ok(new
    {
        WorkItemId = workItemId.Value,
        Details = details,
        Progress = progress,
        ReactorOutput = new
        {
            Summary = summary.Summary.Value,
            MetPlan = summary.Outcome == PlanOutcome.Met
        },
        WaitedForProcessing = true
    });
});

await app.RunAsync();
