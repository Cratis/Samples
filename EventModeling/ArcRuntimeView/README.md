# A coffee counter, viewed from the running app

Build an Arc + Chronicle application and open its event model in your browser.
The example has three slices: place an order, prepare it, and list the orders.
The board describes the application's structure; it is not a live event-log viewer.

## Prerequisites

- .NET SDK **10.0.401** (the repository's `global.json` permits .NET 10 feature-band updates).
- Docker with Compose v2 for the running-app path.
- A browser. No Node.js or frontend build is needed.

NuGet versions are pinned in the repository's `Directory.Packages.props`:
Arc, Arc.Chronicle, Arc.MongoDB, and `Cratis.Arc.Screenplay.Embedded` **22.48.3**;
Chronicle.AspNetCore **19.29.4**. The Compose image is
`cratis/chronicle:19.29.4-development`.

## Open the runtime view — about five minutes

From the repository root, in Bash or another POSIX shell:

```sh
cd EventModeling/ArcRuntimeView
dotnet restore ArcRuntimeView.csproj
dotnet build ArcRuntimeView.csproj --configuration Release --no-restore -warnaserror
docker compose up -d
ASPNETCORE_ENVIRONMENT=Development dotnet run --project ArcRuntimeView.csproj --configuration Release --no-build --no-launch-profile --urls http://localhost:5110
```

Allow Chronicle to finish its first startup. Open
**<http://localhost:5110/.cratis/event-model/>**. Select the application in the
left-hand hierarchy to see the coffee-order commands, their events, and the
orders view. Use the source view to inspect the generated Screenplay.

The host opts in explicitly in [Program.cs](./Program.cs):

```csharp
if (app.Environment.IsDevelopment())
{
    app.MapCratisEventModel();
}
```

Through the `Cratis` metapackage, automatic hosting is development-only by default:
it needs a Debug (non-optimized) build and the Development environment. This
sample builds in Release, so it maps the viewer explicitly, which opts in wherever
the app runs; that is why the call is guarded by `IsDevelopment()`.

`CratisEmbeddedScreenplayEnabled` is explicitly enabled in the project because
the repository disables embedding by default for its other samples. The package
generates and embeds the model during the build. Rebuild after changing a slice.

The host also removes Arc's convention binding for `IChronicleConnection`, so
Chronicle creates its connection from configuration instead of asking dependency
injection to supply its constructor's primitive arguments. This is a bootstrap
workaround for the pinned Arc/Chronicle combination, not an event-model concern.

## Open the same model without starting Chronicle

Stop the app with Ctrl+C, then, from the same directory:

```sh
dotnet tool restore
dotnet cratis view ArcRuntimeView.csproj --configuration Release --port 5112
```

This restores CLI **3.24.0** from `../.config/dotnet-tools.json` and serves the
embedded model from the already-built assembly. It does not run your application
or connect to Chronicle. Open the URL printed by the CLI if the browser does not
open automatically.

If CLI 3.24.0 is already on your PATH, the equivalent command is:

```sh
cratis view ArcRuntimeView.csproj --configuration Release --port 5112
```

## Read the source

- [Ordering](./Coffee/Ordering/PlaceOrder.cs): a model-bound command returns `OrderPlaced`.
- [Preparing](./Coffee/Preparing/PrepareOrder.cs): another command returns `OrderPrepared`.
- [Orders](./Coffee/Orders/Order.cs): Chronicle projects both events; Arc exposes `AllOrders`.
- [OrderId](./Coffee/OrderId.cs): the typed identity selects the order's event stream.

This intentionally small application does not enforce order lifecycle rules,
authentication, or input validation. The preparation time is supplied by the
caller. The viewer is development-only and exposes application structure and
source references; do not expose it or the development database credentials on
a public network. The container ports are bound to loopback.

## Stop and reset

Stop the app and CLI with Ctrl+C. From this sample directory:

```sh
docker compose down --volumes
```

This removes this sample's local Chronicle/MongoDB data. It does not target
containers from the other samples. If ports 5110, 35110, or 27110 are occupied,
stop the conflicting local service before following the quickstart.
