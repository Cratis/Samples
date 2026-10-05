# From a community kitchen to an event-model board

This original Marten + Wolverine application plans a batch of soup, records
prepared portions, and moves a ready batch to the serving table. Generate a
Screenplay model from its C# source, inspect what was recovered, and compare it
with a separately reviewed model. You do not need PostgreSQL to generate or
view the model.

Marten and Wolverine are projects of JasperFx Software; this sample is not affiliated with or endorsed by JasperFx.

## Versions and prerequisites

- .NET SDK **10.0.401**; the repository permits .NET 10 feature-band updates.
- Marten **9.45.0**; WolverineFx, WolverineFx.Marten, WolverineFx.Http, and
  WolverineFx.RuntimeCompilation **6.45.0**.
  These stable NuGet versions are pinned in `../../Directory.Packages.props`.
- Cratis CLI **3.24.0**, restored by the commands below.
- For the first viewing option: VS Code with extension **cratis.screenplay 4.48.0**.
- Optional: Docker with Compose v2 to run the HTTP application.

## Restore, generate, validate, view — under ten minutes

From the repository root:

```sh
cd EventModeling/FromMartenAndWolverine
dotnet tool restore
dotnet restore FromMartenAndWolverine.csproj
dotnet build FromMartenAndWolverine.csproj --configuration Release --no-restore -warnaserror
dotnet cratis screenplay generate FromMartenAndWolverine.csproj --provider critter-stack --domain CommunityKitchen --skip-segments 1 --file Models/Generated/CommunityKitchen.generated.play
dotnet cratis screenplay validate Models/Generated/CommunityKitchen.generated.play --warnings-as-errors
```

With CLI 3.24.0 on PATH, `cratis screenplay generate` and `cratis screenplay validate`
are equivalent to the `dotnet cratis` commands above. The local manifest avoids
relying on whichever global CLI happens to be installed.

Generation emits a diagnostic report: **do not discard it**. This package set is
recognized as source-reviewed, not an exact canonical package set. The generated
file passes validation unchanged, but that only proves valid Screenplay—not that
it faithfully reproduces the program.

### Option 1: VS Code board

```sh
code --install-extension cratis.screenplay@4.48.0
code Models/Generated/CommunityKitchen.generated.play
```

With the `.play` file active, run **Screenplay: Open Event Model Board** from the
command palette. Alternatively, right-click its editor tab, choose **Reopen
Editor With…**, then **Event Model Board**.

You should see three event cards, three command cards, and a `BatchCard` read
view. In the raw output, the commands and events occupy separate slices: the
missing links are an extraction limitation, not the actual application's design.
Pan or zoom out to inspect both generated modules.

Now compare the human-reviewed version:

```sh
dotnet cratis screenplay validate Models/Reviewed/CommunityKitchen.reviewed.play --warnings-as-errors
code Models/Reviewed/CommunityKitchen.reviewed.play
```

It groups the workflow into four slices, supplies the two directly verifiable
command/event mappings, and points `ServeBatch` at its actual aggregate handler.
Its serving decision remains a C# handler, described in prose, rather than an
invented declarative rule. This is a diagram companion, not executable
replacement code. Keep the two model directories separate: compiling both
versions together would duplicate the same domain artifacts.

### Option 2: a board in an AI chat

The local manifest also pins **Cratis.Screenplay.Tool 4.48.0**. Configure an
MCP Apps-capable chat client to launch this stdio server from this sample's
working directory:

```sh
dotnet screenplay mcp Models/Generated
```

For a global installation of the same tool, the command is
`screenplay mcp Models/Generated`. Ask the chat to call **visualize-model** with
no arguments. The client must advertise support for MCP Apps; a text-only MCP
client will not list the visualization tool. Use `Models/Reviewed` as the server
root to compare the reviewed model. No model-writing tool calls are needed.

Use the standalone Screenplay tool for this pinned combination: CLI 3.24.0's
embedded MCP server contains an older Screenplay version and is not the viewer
used here.

### Option 3: anonymous Studio viewer

The anonymous viewer has no file picker or upload. Open the `.play` file in VS Code
with the `cratis.screenplay` extension (Option 1), or, once the file is pushed to
a public repository, open
`https://view.cratis.studio/?url=<encoded raw GitHub URL>` where the URL points to
the raw `.play` file, not its GitHub HTML page. The viewer takes a self-contained
single file and does not follow imports; each file in this sample is one. No
Studio account is required. Put only this public sample—not private application
source—into a hosted viewer.

## What the extraction does and does not tell you

The result is a **reviewed starting point, not a complete reconstruction**.
Human review is required. Sagas, tenancy, middleware, and DCB are recovered
partially; this small application does not exercise those features.

In this sample, generation recovers event and command fields, the `BatchCard`
shape, its query, and the projection's subscribed events. However:

- Explicit `StartStream`/`Events.Append` calls do not become `produces` links.
- The `[AggregateHandler]` serving workflow loses its readiness/duplicate-serving
  guard and the aggregate-loading relationship. The raw handler reference points
  to the HTTP endpoint instead of `ServeBatchHandler`.
- The inline projection becomes an imperative `reducer` reference; field update
  semantics and transaction boundaries are not reconstructed.
- HTTP routes and `IMessageBus.InvokeAsync` request/reply behavior are reported
  as unsupported. `GEN0004`, `WOLVERINE0006`, `MARTEN0003`, and `WOLVERINE0002`
  diagnostics are expected for this sample.
- Putting all types into one namespace can cause a `GEN0007` slice-kind conflict.
  This example uses separate Planning, Preparation, Serving, and Cards slices.

The C# code is authoritative for behavior. The raw generated file is preserved
without manual corrections; every diagram correction is in `Models/Reviewed`.
Neither a clean validation result nor a plausible board is proof of behavioral
equivalence.

## Optional: run the application

The server uses `postgres:17.6-alpine` and a loopback-only port.
`WolverineFx.RuntimeCompilation` supplies the compiler needed for Wolverine's
runtime-generated handlers in this small example. From this sample:

```sh
docker compose up -d --wait
dotnet run --project FromMartenAndWolverine.csproj --configuration Release --no-build --no-launch-profile --urls http://localhost:5111
```

In a second terminal, send this local fixture data:

```sh
curl --fail-with-body -X POST http://localhost:5111/batches -H 'Content-Type: application/json' -d '{"id":"6a6450ec-d5ce-4e8e-84d1-72c82b5c015b","recipe":"Roasted squash soup","targetPortions":12}'
curl --fail-with-body -X POST http://localhost:5111/batches/prepare -H 'Content-Type: application/json' -d '{"id":"6a6450ec-d5ce-4e8e-84d1-72c82b5c015b","portions":12}'
curl --fail-with-body -X POST http://localhost:5111/batches/serve -H 'Content-Type: application/json' -d '{"id":"6a6450ec-d5ce-4e8e-84d1-72c82b5c015b"}'
curl --fail-with-body http://localhost:5111/batches/6a6450ec-d5ce-4e8e-84d1-72c82b5c015b
```

The card should report 12 prepared portions and `served: true`. Calling serve
before enough portions exist, or after the batch was served, is a no-op.
Use a new UUID or reset before repeating the planning request.

The sample omits authentication, input validation, and production concurrency,
retry, and operational policies. It uses local-only demonstration credentials.
Do not deploy it as a production template. No JasperFx sample application was
copied or adapted to create it.

Stop the app with Ctrl+C, then remove **this sample's data**:

```sh
docker compose down --volumes
```
