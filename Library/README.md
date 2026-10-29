# Library showcase

A small, end-to-end library system for exploring how a Cratis application fits together: React frontends, Arc-generated APIs and TypeScript proxies, Chronicle events and projections, MongoDB read models, and Cratis Components.

## What you can explore

The sample presents two focused applications behind local authentication:

| Application | What it looks like |
| --- | --- |
| **Lending** | A desktop-style workspace with a sidebar, paged **Authors** and **Books** tables, and Cratis command dialogs for registering an author or adding a book title. |
| **Members** | A compact **My Library** portal with cards for currently borrowed books and a profile page for the signed-in member. |

The initial event seed supplies recognizable authors, books, members, reservations, and borrowings. Cratis Components 4 supplies the tables, card surfaces, command forms, and dialogs, while Arc React provides the generated command/query hooks.

## Run it with Aspire and MongoDB

From the repository root, install the frontend workspaces once, then start the Library AppHost:

```shell
yarn install
cd Library
./run-mongodb.sh
```

Keep Docker (or a compatible container runtime) running. The repository pins the .NET SDK and Yarn toolchain used by the sample.

The Aspire dashboard opens automatically. When all resources are healthy, use these local endpoints:

| Resource | URL |
| --- | --- |
| Aspire dashboard | <http://localhost:15888> |
| Lending application | <http://localhost:7000> |
| Members application | <http://localhost:7001> |
| Chronicle Workbench | <https://localhost:35000> |

The AppHost starts MongoDB, Chronicle, Vault, two Keycloak realms, both ASP.NET Core backends, both Vite development servers, and the AuthProxy entry points. The credentials below are local sample data only:

| Application | Username | Password |
| --- | --- | --- |
| Lending | `librarian` | `librarian` |
| Lending | `borrower` | `borrower` |
| Members | `alice` | `alice` |
| Members | `bob` | `bob` |

Stop the AppHost with `Ctrl+C`.

## Architecture

```mermaid
flowchart LR
    Browser[Browser]
    LendingProxy[Lending AuthProxy]
    MembersProxy[Members AuthProxy]
    LendingUI[Lending React UI]
    MembersUI[Members React UI]
    Lending[Lending · Arc]
    Members[Members · Arc]
    LibraryStore[(Chronicle · Library store)]
    MembersStore[(Chronicle · Members store)]
    MongoDB[(MongoDB read models)]
    Keycloak[Keycloak realms]

    Browser --> LendingProxy
    Browser --> MembersProxy
    LendingProxy --> LendingUI --> Lending --> LibraryStore --> MongoDB
    MembersProxy --> MembersUI --> Members --> MembersStore --> MongoDB
    LendingProxy -. sign in .-> Keycloak
    MembersProxy -. sign in .-> Keycloak
```

- **React + Cratis Components** render commands, observable queries, tables, dialogs, and member-facing cards.
- **Arc** discovers model-bound commands and queries, exposes their HTTP endpoints, and generates the colocated TypeScript proxies consumed by React.
- **Chronicle** stores domain events and runs projections and reducers into MongoDB read models.
- **Aspire Composition** wires infrastructure, authentication, applications, endpoints, startup order, and observability into one local run.
- **Lending.Contracts** holds the borrowing integration events shared by the two application domains.

## Project map

```text
Library/
├── Composition/        Aspire AppHost and local Keycloak realm imports
├── Lending/            Librarian UI, authors, inventory, lending behavior, and specs
├── Lending.Contracts/  Borrowing integration event contracts
├── Members/            Member portal, identity, profiles, and borrowed-book views
├── Library.slnx        Application solution
└── run-mongodb.sh      Recommended local entry point
```

Backend and frontend files live together by behavior rather than in separate technical trees. For example:

```text
Lending/Authors/
├── AuthorId.cs
├── AuthorName.cs
├── Registration/
│   ├── Registration.cs       command, event, and uniqueness constraint
│   ├── RegisterAuthor.ts     generated Arc command proxy
│   ├── AddAuthor.tsx         Cratis Components command dialog
│   └── when_registering/     command specifications
└── Listing/
    ├── Listing.cs            read model, observable query, and projection
    ├── ObserveAll.ts         generated Arc query proxy
    ├── Listing.tsx           paged React table
    └── for_author_projection/ projection specifications
```

Concept types make domain intent visible at every boundary: `AuthorName` is a `ConceptAs<string>`, `ISBN` is an `EventSourceId`, and identifiers such as `AuthorId` and `MemberId` are dedicated types rather than loose primitives. A Debug build regenerates the TypeScript commands, queries, and models used by each React slice.

## Read protection in Lending

The Lending project sets `<CratisReadProtection>On</CratisReadProtection>` in [`Lending.csproj`](./Lending/Lending.csproj); Arc's default is Off. Under On, a read model that a command or its validator takes as a parameter is read fresh from the event log and guards that command's append.

Because the command declares read protection On, a caller cannot skip these state rules by sending a lower validation threshold: an `X-Allowed-Severity` of Error or Unknown still blocks the command on an Error or Unknown validation result such as `BorrowBookValidator`'s availability check.

The availability rule lives in [`BorrowBookValidator`](./Lending/Lending/Borrowing/Borrowing.cs) and [`ReserveBookValidator`](./Lending/Lending/Reserving/Reserving.cs). Each takes `BookAvailability` in its constructor and refuses the command when no copy is available; `Handle()` only returns the event. If an inventory, reservation, borrow or return event for the same book arrives after the read, nothing from the command is written and the caller gets HTTP 400 with a validation result whose reason is `concurrencyViolation` and whose detail is `ReadChanged`. Arc and Chronicle do not re-run the command; the caller reads the new availability and submits again. The guard covers the event types `BookAvailability` is reduced from, for that ISBN. It does not cover revisions or redactions, other event sequences, or services and stores outside Chronicle.

[`when_the_last_copy_is_borrowed_concurrently`](./Lending/Lending/Borrowing/for_BorrowBook/when_the_last_copy_is_borrowed_concurrently.cs) specifies the lost race with `CommandScenario.AppendConcurrently`.

## Build and verify

From the Samples repository root:

```bash
dotnet build Library/Library.slnx --configuration Debug
dotnet run --project Library/Lending/Lending.csproj --configuration Debug --no-build -- --specifications -class "*.for_*"
dotnet run --project Library/Members/Members.csproj --configuration Debug --no-build -- --specifications
dotnet build Library/Library.slnx --configuration Release -p:CratisProxiesOutputPath=
yarn --cwd Library/Lending build
yarn --cwd Library/Members build
```

## Ideas to try

- Register an author, then add a book and select that author in the generated command form.
- Keep the Authors or Books table open while changing data and observe the query update.
- Open Chronicle Workbench and follow `AuthorRegistered` or `BookAddedToInventory` from event to projection.
- Compare `Registration.cs`, its generated `RegisterAuthor.ts` proxy, and `AddAuthor.tsx` to see the full-stack type flow.
- Follow the static `Reduce` overloads on [`BookAvailability`](./Lending/Lending/Reserving/Reserving.cs) and [`BorrowedBook`](./Members/BorrowedBooks/BorrowedBooks.cs), then inspect the borrowing event contracts on each side. These pure folds are self-reducing read models; keep a separate `IReducerFor<T>` for injected services, replay-instance behavior, or a separate model contract. Arc excludes the methods taking their own model type from query discovery; intended query methods remain on the same record.
- The specifications live beside the code in each app. Run them from this directory with the Debug-built app, `dotnet run --project <Lending|Members>/<Lending|Members>.csproj --configuration Debug --no-build -- --specifications`. Lending's `-class "*.for_*"` filter selects its in-process specs; running its full suite also requires Chronicle and MongoDB.
