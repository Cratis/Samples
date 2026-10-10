<div align="center">

# Learning path

### Learn event sourcing with Cratis one small, runnable step at a time

**Chronicle · Arc · .NET**

[Back to all samples](../README.md)

</div>

---

## How the path works

Each step is a small, self-contained .NET project that adds one idea to the step before it. All steps use the same shopping cart, so you can focus on what changes:

```text
01  events             append → event log → read the history back
02  projection         events → projection → read model
03  commands/queries   HTTP command → event → projection → HTTP query
```

The steps don't share code. Each one copies the cart types it needs, so you can open any folder and read it top to bottom without jumping around.

## The steps

| Step | You learn | Concepts guide |
| --- | --- | --- |
| [01 — First events](./01-FirstEvents/README.md) | Define events, append them to the event log, and read them back | [What is event sourcing?](https://www.cratis.io/concepts/event-sourcing/) |
| [02 — First projection](./02-FirstProjection/README.md) | Build a read model from those events and query it | [Projections and read models](https://www.cratis.io/concepts/projections-and-read-models/) |
| [03 — Commands and queries with Arc](./03-CommandsAndQueries/README.md) | Append events from an Arc command and return the read model from an Arc query | [CQRS explained](https://www.cratis.io/concepts/cqrs/) |

## Before you start

You need the [.NET 10 SDK](https://dotnet.microsoft.com/download), [Docker](https://www.docker.com/), and `curl`.

Every step talks to a local Chronicle server. Start the development image once and leave it running while you work through the steps:

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
```

Building does not need Chronicle. Running does.

## Coming later

The first steps stop at commands and queries. Later steps will cover reacting to events with reactors, changing event shapes over time, testing, and preparing for production. Until then, the [Chronicle Processing](../Chronicle/Processing/README.md) sample shows reactors and reducers, and the [Capstone](../Capstone/README.md) sample puts Arc, Chronicle, and React together.

> [!NOTE]
> These steps are for learning. They leave out validation, authentication, tenancy, and production configuration so each idea stays in view.

## Questions? Ask on Discord

Ask questions and get help from the Cratis team and other developers on the [Cratis Discord](https://discord.gg/kt4AMpV8WV). The [Cratis community page](https://www.cratis.io/community/) lists other ways to take part.
