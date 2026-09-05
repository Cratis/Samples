// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle;
using OperationsDiagnosis;

const string EventStoreName = "OperationsDiagnosisSample";

var builder = WebApplication.CreateBuilder(args)
    .AddCratisChronicle(options => options.EventStore = EventStoreName);

var app = builder.Build();
app.UseCratisChronicle();

app.MapGet("/", () => Results.Ok(new
{
    Sample = "Chronicle operations diagnosis",
    Trigger = "POST /fixture/failure",
    ProbeId = ProbeId.FailureFixture.Value
}));

app.MapPost("/fixture/failure", async (IEventStore eventStore) =>
{
    var appendResult = await eventStore.EventLog.Append(
        ProbeId.FailureFixture,
        new ProbeRequested(ProbeName.FailureFixture));

    if (!appendResult.IsSuccess)
    {
        return Results.Problem("Chronicle rejected the diagnostic probe event.");
    }

    return Results.Accepted(
        "/fixture/failure",
        new
        {
            EventStore = EventStoreName,
            Namespace = "Default",
            EventSourceId = ProbeId.FailureFixture.Value,
            EventType = nameof(ProbeRequested),
            ExpectedFailure = nameof(ProbeConfiguredToFail)
        });
});

await app.RunAsync("http://localhost:5078");
