<div align="center">

# Learning path

### Learn event sourcing with Cratis one small, runnable step at a time

**Chronicle · Arc · .NET**

[Back to all samples](../README.md)

</div>

---

## How the path works

Each step is a small, self-contained .NET project that adds one idea to the step before it. All steps use the same shopping cart, from recording its first events to sending a confirmation after checkout, so you can focus on what changes:

```text
01  events             append → event log → read the history back
02  projection         events → projection → read model
03  commands/queries   HTTP command → event → projection → HTTP query
04  reactors           checkout event → reactor → confirmation + follow-up event
05  testing            decision + command pipeline + projection + reactor → specs
06  changing events    old + new event generations → migration → priced cart lines
07  validation/access  identity → input rules → protected decision → event or rejection
08  production         trusted TLS + JWT identity + health probes + distributed traces
```

The steps don't share code. Each one copies the cart types it needs, so you can open any folder and read it top to bottom without jumping around.

## The steps

| Step | You learn | Concepts guide |
| --- | --- | --- |
| [01 — First events](./01-FirstEvents/README.md) | Define events, append them to the event log, and read them back | [What is event sourcing?](https://www.cratis.io/concepts/event-sourcing/) |
| [02 — First projection](./02-FirstProjection/README.md) | Build a read model from those events and query it | [Projections and read models](https://www.cratis.io/concepts/projections-and-read-models/) |
| [03 — Commands and queries with Arc](./03-CommandsAndQueries/README.md) | Append events from an Arc command and return the read model from an Arc query | [CQRS explained](https://www.cratis.io/concepts/cqrs/) |
| [04 — Reactors](./04-Reactors/README.md) | React to checkout with a side effect and return a follow-up event | [Event-driven architecture](https://www.cratis.io/concepts/event-driven-architecture/) |
| [05 — Testing](./05-Testing/README.md) | Specify the cart's decisions, appended events, read model, and reaction without a server | [Chronicle testing](https://www.cratis.io/chronicle/testing/) |
| [06 — Event versioning](./06-EventVersioning/README.md) | Evolve an event through generations and query old and new cart history | [Event type migrations](https://www.cratis.io/chronicle/concepts/event-type-migrations/) |
| [07 — Validation and authorization](./07-ValidationAndAuthorization/README.md) | Reject bad input, protect decisions after checkout, and require a customer role | [Command validation](https://github.com/Cratis/Arc/blob/main/Documentation/backend/csharp/commands/validation.md) |
| [08 — Preparing for production](./08-Production/README.md) | Run the production image with TLS, JWT authentication, health probes, and telemetry | [Production hosting](https://github.com/Cratis/Chronicle/blob/main/Documentation/hosting/production.md) |

## Before you start

You need the [.NET 10 SDK](https://dotnet.microsoft.com/download), [Docker](https://www.docker.com/), and `curl`.

Running the step applications talks to a local Chronicle server. Step 05, step 07, and step 08's specs need only the .NET SDK: no Docker, API, or Chronicle server. Step 08 runs its own production-shaped Compose stack; follow its README instead of starting the development server below. For steps 01–07, start the development image once and leave it running while you work through the steps:

```bash
docker run -d --name chronicle \
  -p 127.0.0.1:35000:35000 \
  -p 127.0.0.1:27017:27017 \
  cratis/chronicle:latest-development
```

Wait until it reports healthy:

```bash
curl --insecure --fail --retry 30 --retry-all-errors --retry-delay 1 https://localhost:35000/health
```

The development image bundles MongoDB for read models. Chronicle Workbench runs on the same port at <https://localhost:35000>; sign in with username `Admin` and password `ChangeMeNow!` (see [Workbench development](https://www.cratis.io/chronicle/workbench/development/)). Each step uses its own event store, so you can see what each one wrote.

Remove the container when you are done:

```bash
docker rm -f chronicle
```

## Build every step

From the repository root:

```bash
dotnet build LearningPath/01-FirstEvents/FirstEvents.csproj
dotnet build LearningPath/02-FirstProjection/FirstProjection.csproj
dotnet build LearningPath/03-CommandsAndQueries/CommandsAndQueries.csproj
dotnet build LearningPath/04-Reactors/Reactors.csproj
dotnet build LearningPath/05-Testing/Testing.csproj
dotnet test LearningPath/05-Testing/Testing.Specs/Testing.Specs.csproj --configuration Debug
dotnet build LearningPath/06-EventVersioning/EventVersioning.csproj
dotnet build LearningPath/07-ValidationAndAuthorization/ValidationAndAuthorization.csproj
dotnet test LearningPath/07-ValidationAndAuthorization/ValidationAndAuthorization.Specs/ValidationAndAuthorization.Specs.csproj --configuration Debug
dotnet build LearningPath/08-Production/Production.csproj
dotnet test LearningPath/08-Production/Production.Specs/Production.Specs.csproj --configuration Debug
```

Building and running the specs do not need Chronicle. Running the step applications does.

## Explore further

The [Chronicle Processing](../Chronicle/Processing/README.md) sample compares reactors, projections, and reducers, and the [Capstone](../Capstone/README.md) sample puts Arc, Chronicle, and React together.

> [!NOTE]
> These steps are for learning. Steps 01–06 omit validation and authentication; step 07 adds rules and a development-only identity fixture, not production sign-in. Step 08 replaces that fixture with JWT validation and adds production configuration. Tenancy and cart ownership remain outside the path.

## Questions? Ask on Discord

Ask questions and get help from the Cratis team and other developers on the [Cratis Discord](https://discord.gg/kt4AMpV8WV). The [Cratis community page](https://www.cratis.io/community/) lists other ways to take part.
