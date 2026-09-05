<div align="center">

# Cratis Samples

### Runnable event sourcing and CQRS samples for the Cratis stack

From a small Chronicle event-sourcing process to a React application composed with Arc, Components, and Aspire — covering the Chronicle event store, Arc CQRS for ASP.NET Core, React Components, the CLI, Workbench, and the model-first Screenplay and Stage (experimental).

[![Build](https://github.com/Cratis/Samples/actions/workflows/build.yml/badge.svg)](https://github.com/Cratis/Samples/actions/workflows/build.yml)
[![Update dependencies](https://github.com/Cratis/Samples/actions/workflows/update-dependencies.yml/badge.svg)](https://github.com/Cratis/Samples/actions/workflows/update-dependencies.yml)
[![Documentation](https://github.com/Cratis/Samples/actions/workflows/documentation.yml/badge.svg)](https://github.com/Cratis/Samples/actions/workflows/documentation.yml)

[Explore the samples](#pick-a-sample) · [Run locally](#run-locally) · [Browse the documentation](https://cratis.io/samples/)

</div>

---

## Pick a sample

| Sample | Experience | Products | Start here |
| --- | --- | --- | --- |
| **[Chronicle Backend](./Chronicle/Backend/README.md)** | HTTP API | Chronicle | Append one immutable fact and read an event source's history. |
| **[Chronicle TypeScript Client](./Chronicle/TypeScript/README.md)** | Terminal | Chronicle | Append a fact from Node.js, let a reactor respond, and read the history. |
| **[Chronicle Processing](./Chronicle/Processing/README.md)** | HTTP API | Chronicle, Fundamentals | Compare a projection, reducer, and reactor on one event stream. |
| **[Idea Loom — Arc + React](./Arc/React/README.md)** | React | Arc, Components, Fundamentals | Follow a typed command and observable query from C# to a polished UI. |
| **[Chronicle Multi-Tenancy](./Chronicle/MultiTenancy/README.md)** | HTTP API | Arc, Chronicle, Fundamentals | Isolate the same typed workflow across tenant namespaces. |
| **[Chronicle Cross-Store](./Chronicle/CrossStore/README.md)** | HTTP API | Chronicle, Fundamentals | Connect two event stores through an outbox, inbox, and local translation. |
| **[Chronicle Operations Diagnosis](./Chronicle/OperationsDiagnosis/README.md)** | CLI + Workbench | Chronicle, CLI, Fundamentals | Create one known failure and learn to inspect it before repair. |
| **[Chronicle with ASP.NET Core](./Chronicle/Quickstart/AspNetCore/README.md)** | HTTP API | Chronicle | Host Chronicle through dependency injection and expose focused endpoints. |
| **[Model-First Library](./ModelFirst/Library/README.md)** | Executable model | Screenplay, Stage | Compile one modeled workflow and run its accepted and rejected examples. |
| **[Library](./Library/README.md)** | React + APIs | Arc, Chronicle, Components | Explore separate lending and membership applications under one local composition. |

> [!TIP]
> Start with Chronicle Backend for the smallest HTTP path, then open Chronicle Processing to compare projections, reducers, and reactors. Move to Library when you want generated TypeScript contracts and React.

## What the journey looks like

```text
Arc:         command → current-state store → observable query → React
Chronicle:   append → event history → projections / reactions → views
Model-first: .play model → Screenplay compiler → Stage specifications
```

Choose the path matching what you want to learn. The focused samples keep one idea in view; Library brings several paths together in a larger application.

- **Arc** — CQRS for ASP.NET Core: typed commands, observable queries, and TypeScript proxy generation. [Arc documentation](https://www.cratis.io/arc/)
- **Chronicle** — the event store: append immutable events, then build projections and reactions over the history. [Chronicle documentation](https://www.cratis.io/chronicle/)
- **Model-first (experimental)** — describe a workflow in a [Screenplay](https://github.com/Cratis/Screenplay) model and let [Stage](https://github.com/Cratis/Stage) render it into a runnable application.

## Run locally

### You will need

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) or another compatible container runtime
- Node.js 23 or newer for the React applications
- Corepack/Yarn for frontend work

Clone the repository and validate the catalog:

```bash
git clone https://github.com/Cratis/Samples.git
cd Samples
corepack enable
yarn install
yarn samples:validate
yarn lint:ci
yarn build
```

Build the .NET samples:

```bash
dotnet restore Samples.slnx
dotnet build Samples.slnx --configuration Debug
dotnet test Samples.slnx --configuration Debug --no-build
dotnet build Samples.slnx --configuration Release -p:CratisProxiesOutputPath=
```

Each sample README contains its own infrastructure and run commands, a short tour of the code, and ideas to try next.

## How the repository is organized

```text
Arc/             standalone Arc and React samples
Chronicle/       focused Chronicle samples
ModelFirst/      executable Screenplay and Stage samples
Library/         the larger React and multi-service showcase
samples.json     the catalog used by repository checks and the documentation site
scripts/         catalog and repository checks
```

The catalog is deliberately machine-readable. The [Cratis documentation sample roster](https://cratis.io/samples/) is generated from it so descriptions and source links stay aligned with the runnable projects. A sample can add `previewUrl` and `previewLabel` when a real read-only experience, such as an event-model viewer, is available.

## Samples are for learning

These projects favor clarity and visible behavior. They demonstrate exact, documented combinations of Cratis packages and local infrastructure; they are not production templates or support commitments. Every sample calls out the pieces it intentionally leaves out.

## Contributing a sample

A sample should be enjoyable to explore and easy to understand:

1. Give it one clear learning goal.
2. Keep it independently runnable.
3. Include an inviting `README.md` with an architecture sketch, exact commands, expected behavior, and a few ideas to try.
4. Add tests for the behavior the sample teaches.
5. Add the entry to [`samples.json`](./samples.json).
6. Run `yarn samples:validate` and the sample's own build and test commands.

## The wider Cratis ecosystem

These samples are part of [Cratis](https://www.cratis.io) — free, MIT-licensed tools for building event-sourced and CQRS applications.

- **[Chronicle](https://github.com/Cratis/Chronicle)** — event-sourcing database and runtime. Orleans-based kernel, pluggable storage (MongoDB default; PostgreSQL, SQL Server, SQLite, in-memory), language-agnostic gRPC contracts. [Docs](https://www.cratis.io/chronicle/)
- **Chronicle clients** — first-class [.NET SDK](https://github.com/Cratis/Chronicle), plus [TypeScript](https://github.com/Cratis/Chronicle.TypeScript), [Kotlin/Java](https://github.com/Cratis/Chronicle.Kotlin), and [Elixir](https://github.com/Cratis/Chronicle.Elixir); [Python](https://github.com/Cratis/Chronicle.Python) coming soon (pre-alpha). AI agents connect through the [Chronicle MCP server](https://github.com/Cratis/Chronicle.Mcp).
- **[Arc](https://github.com/Cratis/Arc)** — opinionated CQRS framework for ASP.NET Core with commands, queries, validation, authorization, and TypeScript proxy generation. Works without event sourcing. [Docs](https://www.cratis.io/arc/)
- **[Components](https://github.com/Cratis/Components)** — React components aligned with Arc patterns. [Docs](https://www.cratis.io/components/)
- **[CLI](https://github.com/Cratis/cli) + Workbench** — inspect and diagnose Chronicle from the terminal or the browser. [Docs](https://www.cratis.io/cli/)
- **Model-first layer (experimental)** — [Studio](https://github.com/Cratis/Studio), [Screenplay](https://github.com/Cratis/Screenplay), [Stage](https://github.com/Cratis/Stage), [Scene](https://github.com/Cratis/Scene), [Prologue](https://github.com/Cratis/Prologue)
- **Supporting** — [Fundamentals](https://github.com/Cratis/Fundamentals), [Specifications](https://github.com/Cratis/Specifications), [Synopsis](https://github.com/Cratis/Synopsis), [Lens](https://github.com/Cratis/Lens), [Narrator](https://github.com/Cratis/Narrator), and free [AI tooling](https://github.com/Cratis/AI) (preview); [Ensemble](https://github.com/Cratis/Ensemble) coming soon (pre-release)

Everything Cratis publishes today is MIT licensed and free to use.

---

<div align="center">

**Build something, change it, and watch the model and runtime tell the same story.**

</div>
