<div align="center">

# Chronicle with ASP.NET Core

### Put a focused HTTP surface in front of Chronicle

**Chronicle · ASP.NET Core · Minimal APIs · MongoDB**

[Back to all samples](../../../README.md) · [Start with Chronicle Backend](../../Backend/README.md)

</div>

---

## What you will explore

This sample hosts a small library event model and its read-side artifacts through ASP.NET Core dependency injection.

```text
HTTP request
    │
    ▼
minimal endpoint ──► Chronicle event log ──► processing ──► materialized view
```

It adds reducers, projections, reactors, MongoDB-backed views, and focused HTTP endpoints to the smaller [Chronicle Backend](../../Backend/README.md) starting point.

## Run it

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) or another compatible container runtime
- `curl`

Start from the sample directory:

```bash
cd Chronicle/Quickstart/AspNetCore
docker compose -f ../docker-compose.yml up -d
```

Start the API:

```bash
dotnet run
```

The application listens on <http://localhost:5000>.

## Build it

```bash
dotnet build AspNetCore.csproj --configuration Debug
```

This focused host currently has no dedicated test project.

## Send a couple of events

Record a reservation:

```bash
curl --request POST http://localhost:5000/api/books/reserve
```

Record a borrowing event with explicit sample identifiers:

```bash
curl --request POST \
  http://localhost:5000/api/books/11111111-1111-1111-1111-111111111111/borrow/22222222-2222-2222-2222-222222222222
```

Then open Chronicle Workbench at <http://localhost:8080> and select the `Quickstart` event store to inspect the events and processing state.

## Code tour

| Area | Location |
| --- | --- |
| Chronicle host setup | [`Program.cs`](./Program.cs) |
| HTTP endpoints | [`../Common.AspNetCore/Api.cs`](../Common.AspNetCore/Api.cs) |
| Dependency registration | [`../Common.AspNetCore/CommonServices.cs`](../Common.AspNetCore/CommonServices.cs) |
| MongoDB registration | [`../Common.AspNetCore/MongoDBServices.cs`](../Common.AspNetCore/MongoDBServices.cs) |
| Shared events and read-side artifacts | [`../Common`](../Common/) |

The key host setup is deliberately short:

```csharp
var builder = WebApplication.CreateBuilder(args)
    .AddCratisChronicle(options => options.EventStore = "Quickstart");

var app = builder.Build();
app.UseCratisChronicle();
```

## Try changing it

1. Add a `GET` endpoint that answers a specific library question.
2. Add validation before recording a borrowing event.
3. Introduce one new event and follow its processing in Workbench.
4. Compare the processing setup with the focused [Chronicle Processing](../../Processing/README.md) sample.

## Stop the infrastructure

```bash
docker compose -f ../docker-compose.yml down
```

> [!NOTE]
> This sample focuses on Chronicle hosting and event append. It does not include Arc-generated endpoints, React, tenancy, or cross-store messaging.
