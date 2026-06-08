# 📚 Library Sample

This sample shows a full Library system built on the entire Cratis Stack across two independent microservices — **Lending** and **Members** — composed via an Aspire AppHost with the complete local infrastructure.

## 🚀 Quick Start

```shell
cd Library
./run-mongodb.sh
```

Once everything is up, open the Aspire dashboard at [http://localhost:15888](http://localhost:15888) to watch all services start. Then visit:

- **Lending app** → [http://localhost:7000](http://localhost:7000)
- **Members app** → [http://localhost:7001](http://localhost:7001)
- **Chronicle Workbench** → [http://localhost:8080](http://localhost:8080) — browse the event log, projections, and read models

Log in at the Keycloak prompt or use development headers to skip authentication (see [Identity & Multi-Tenancy](#-identity--multi-tenancy) below).

---

## 🔐 Identity & Multi-Tenancy

Both **Lending** and **Members** implement the Cratis Arc identity system, which gives you two ways to work as a specific user:

1. **Keycloak (full auth flow)** — go through the Keycloak login page.
2. **Development headers (no auth required)** — set HTTP headers to impersonate any user directly. This works when accessing the backends or frontends directly (ports 5000/5001 and 9000/9001), bypassing the AuthProxy.

Multi-tenancy is also showcased: each service exposes two library branches as tenants (`central` and `westside`), selectable without any external infrastructure when working in development mode.

### Logging in via Keycloak

When you open the app through the AuthProxy (ports 7000/7001), Keycloak handles authentication. Use these pre-configured accounts:

**Lending realm** ([http://localhost:8090/realms/lending](http://localhost:8090/realms/lending))

| Username | Password | Role |
|----------|----------|------|
| `librarian` | `librarian` | Head Librarian |
| `borrower` | `borrower` | Regular Borrower |

**Members realm** ([http://localhost:8091/realms/members](http://localhost:8091/realms/members))

| Username | Password | Name |
|----------|----------|------|
| `alice` | `alice` | Alice Smith |
| `bob` | `bob` | Bob Jones |

### Development Identity — No Auth Required

When running without the AuthProxy (direct access on ports 5000/5001 and 9000/9001), the Arc identity system reads the Microsoft Identity Platform headers instead of a real JWT. Set these three headers on every request to impersonate a user:

| Header | Purpose |
|--------|---------|
| `X-MS-CLIENT-PRINCIPAL-ID` | User's unique identifier (GUID) |
| `X-MS-CLIENT-PRINCIPAL-NAME` | User's display name |
| `X-MS-CLIENT-PRINCIPAL` | Base64-encoded JSON claims payload |

#### Members users

##### Alice Smith

```text
X-MS-CLIENT-PRINCIPAL-ID: 00000000-1000-0000-0000-000000000001
X-MS-CLIENT-PRINCIPAL-NAME: Alice Smith
X-MS-CLIENT-PRINCIPAL: eyJhdXRoX3R5cCI6ImFhZCIsImNsYWltcyI6W3sidHlwIjoiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiLCJ2YWwiOiIwMDAwMDAwMC0xMDAwLTAwMDAtMDAwMC0wMDAwMDAwMDAwMDEifSx7InR5cCI6Im5hbWUiLCJ2YWwiOiJBbGljZSBTbWl0aCJ9LHsidHlwIjoicHJlZmVycmVkX3VzZXJuYW1lIiwidmFsIjoiYWxpY2VAbWVtYmVycy5sb2NhbCJ9XSwibmFtZV90eXAiOiJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIiwicm9sZV90eXAiOiJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUifQ==
```

##### Bob Jones

```text
X-MS-CLIENT-PRINCIPAL-ID: 00000000-1000-0000-0000-000000000002
X-MS-CLIENT-PRINCIPAL-NAME: Bob Jones
X-MS-CLIENT-PRINCIPAL: eyJhdXRoX3R5cCI6ImFhZCIsImNsYWltcyI6W3sidHlwIjoiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiLCJ2YWwiOiIwMDAwMDAwMC0xMDAwLTAwMDAtMDAwMC0wMDAwMDAwMDAwMDIifSx7InR5cCI6Im5hbWUiLCJ2YWwiOiJCb2IgSm9uZXMifSx7InR5cCI6InByZWZlcnJlZF91c2VybmFtZSIsInZhbCI6ImJvYkBtZW1iZXJzLmxvY2FsIn1dLCJuYW1lX3R5cCI6Imh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiLCJyb2xlX3R5cCI6Imh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSJ9
```

#### Lending users

##### Head Librarian

```text
X-MS-CLIENT-PRINCIPAL-ID: 00000000-2000-0000-0000-000000000001
X-MS-CLIENT-PRINCIPAL-NAME: Head Librarian
X-MS-CLIENT-PRINCIPAL: eyJhdXRoX3R5cCI6ImFhZCIsImNsYWltcyI6W3sidHlwIjoiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiLCJ2YWwiOiIwMDAwMDAwMC0yMDAwLTAwMDAtMDAwMC0wMDAwMDAwMDAwMDEifSx7InR5cCI6Im5hbWUiLCJ2YWwiOiJIZWFkIExpYnJhcmlhbiJ9LHsidHlwIjoicHJlZmVycmVkX3VzZXJuYW1lIiwidmFsIjoibGlicmFyaWFuQGxpYnJhcnkubG9jYWwifV0sIm5hbWVfdHlwIjoiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSIsInJvbGVfdHlwIjoiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIn0=
```

##### Regular Borrower

```text
X-MS-CLIENT-PRINCIPAL-ID: 00000000-2000-0000-0000-000000000002
X-MS-CLIENT-PRINCIPAL-NAME: Regular Borrower
X-MS-CLIENT-PRINCIPAL: eyJhdXRoX3R5cCI6ImFhZCIsImNsYWltcyI6W3sidHlwIjoiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiLCJ2YWwiOiIwMDAwMDAwMC0yMDAwLTAwMDAtMDAwMC0wMDAwMDAwMDAwMDIifSx7InR5cCI6Im5hbWUiLCJ2YWwiOiJSZWd1bGFyIEJvcnJvd2VyIn0seyJ0eXAiOiJwcmVmZXJyZWRfdXNlcm5hbWUiLCJ2YWwiOiJib3Jyb3dlckBsaWJyYXJ5LmxvY2FsIn1dLCJuYW1lX3R5cCI6Imh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiLCJyb2xlX3R5cCI6Imh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSJ9
```

#### Example: calling the API as Alice

```shell
curl -s http://localhost:5001/api/members/profiles \
  -H "X-MS-CLIENT-PRINCIPAL-ID: 00000000-1000-0000-0000-000000000001" \
  -H "X-MS-CLIENT-PRINCIPAL-NAME: Alice Smith" \
  -H "X-MS-CLIENT-PRINCIPAL: eyJhdXRoX3R5cCI6ImFhZCIsImNsYWltcyI6W3sidHlwIjoiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiLCJ2YWwiOiIwMDAwMDAwMC0xMDAwLTAwMDAtMDAwMC0wMDAwMDAwMDAwMDEifSx7InR5cCI6Im5hbWUiLCJ2YWwiOiJBbGljZSBTbWl0aCJ9LHsidHlwIjoicHJlZmVycmVkX3VzZXJuYW1lIiwidmFsIjoiYWxpY2VAbWVtYmVycy5sb2NhbCJ9XSwibmFtZV90eXAiOiJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIiwicm9sZV90eXAiOiJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUifQ=="
```

See the [Microsoft Identity Platform headers reference](https://www.cratis.io/arc/backend/asp-net-core/microsoft-identity/) for a full explanation of how Arc reads these headers.

### Lens — Switch Users & Tenants in the Browser

> **Direct access only (ports 9000/9001).** Lens injects `X-MS-CLIENT-PRINCIPAL-*` headers, which are ignored when going through the AuthProxy (ports 7000/7001) — those ports authenticate via Keycloak. Use Lens when accessing the Vite dev server directly.

[**Lens**](https://github.com/cratis/lens) is a browser extension that injects the `X-MS-CLIENT-PRINCIPAL-*` headers automatically on every request, and lets you switch between pre-defined users and tenants from a toolbar popup. It eliminates the need to manually craft headers when exploring the apps in the browser.

Install Lens, configure the user list for each service (using the IDs and base64 payloads above), and switch identity with a single click — no Keycloak login required when accessing the frontend apps directly.

### Multi-Tenancy

Both services expose two tenants representing branches of the same library system:

| Tenant ID | Display Name |
|-----------|-------------|
| `central` | Central Library |
| `westside` | Westside Branch |

To target a specific tenant, set the `Tenant-ID` header on your request (or select the tenant in Lens):

```shell
curl -s http://localhost:5000/api/lending/inventory \
  -H "Tenant-ID: westside" \
  -H "X-MS-CLIENT-PRINCIPAL-ID: 00000000-2000-0000-0000-000000000001" \
  -H "X-MS-CLIENT-PRINCIPAL-NAME: Head Librarian" \
  -H "X-MS-CLIENT-PRINCIPAL: eyJhdXRoX3R5cCI6ImFhZCIsImNsYWltcyI6W3sidHlwIjoiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiLCJ2YWwiOiIwMDAwMDAwMC0yMDAwLTAwMDAtMDAwMC0wMDAwMDAwMDAwMDEifSx7InR5cCI6Im5hbWUiLCJ2YWwiOiJIZWFkIExpYnJhcmlhbiJ9LHsidHlwIjoicHJlZmVycmVkX3VzZXJuYW1lIiwidmFsIjoibGlicmFyaWFuQGxpYnJhcnkubG9jYWwifV0sIm5hbWVfdHlwIjoiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSIsInJvbGVfdHlwIjoiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIn0="
```

### How It Works — ICanProvideUsers & ICanProvideTenants

The Arc identity system discovers implementations of `ICanProvideUsers` and `ICanProvideTenants` automatically at startup (via `IInstancesOf<T>`). In debug builds, each service registers its own set:

**`Members/Development/Users.cs`** — provides Alice and Bob to the Arc dev tooling:

```csharp
#if DEBUG
public class Users : ICanProvideUsers
{
    public Task<IEnumerable<User>> Provide() => Task.FromResult<IEnumerable<User>>(
    [
        CreateUser("00000000-1000-0000-0000-000000000001", "Alice Smith", "alice@members.local"),
        CreateUser("00000000-1000-0000-0000-000000000002", "Bob Jones",   "bob@members.local"),
    ]);
}
#endif
```

**`Members/Development/Tenants.cs`** — provides the two library branches:

```csharp
#if DEBUG
public class Tenants : ICanProvideTenants
{
    public Task<IEnumerable<Tenant>> Provide() => Task.FromResult<IEnumerable<Tenant>>(
    [
        new Tenant("central",  "Central Library"),
        new Tenant("westside", "Westside Branch"),
    ]);
}
#endif
```

Lending has identical files under `Lending/Development/` with its own set of users (librarian and borrower). Because both files are wrapped in `#if DEBUG`, they are stripped from Release builds automatically.

---

## 🏛️ Architecture

```text
Library/
├── Lending/       — Book lending domain (catalogue, loans, reservations)
├── Members/       — Member management domain (registration, profiles)
└── Composition/   — Aspire AppHost wiring the full local stack
```

### Infrastructure (Composition)

| Service | Description |
|---------|-------------|
| **Chronicle** | Event-sourced kernel — stores all events and drives projections |
| **HashiCorp Vault** | Dev-mode secret store for Chronicle compliance encryption keys |
| **Keycloak (lending)** | OIDC provider for the Lending realm (librarian / borrower users) |
| **Keycloak (members)** | OIDC provider for the Members realm (alice / bob users) |
| **AuthProxy (lending)** | Authenticates and proxies requests to the Lending backend + frontend |
| **AuthProxy (members)** | Authenticates and proxies requests to the Members backend + frontend |

## 🍕 Vertical Slices

Both microservices are structured around vertical slices — each feature folder contains backend C#, TypeScript/React frontend, and automated specs side by side.

## 📋 Pre-requisites

- [.NET 10](https://dot.net)
- [Node.js](https://nodejs.org) + [Yarn](https://yarnpkg.com)
- [Docker](https://www.docker.com/products/docker-desktop/) or compatible container engine

## 🗄️ Database Backends

Chronicle supports multiple storage backends. The Composition AppHost and docker-compose files both support the same set of profiles. **MongoDB is the default** when no database type is specified.

| Database | DATABASE_TYPE value | Notes |
|----------|---------------------|-------|
| MongoDB | `mongodb` _(default)_ | Uses Chronicle development image (embedded MongoDB) |
| PostgreSQL | `postgresql` | Starts a `postgres:16` container |
| Microsoft SQL Server | `mssql` | Starts `mcr.microsoft.com/mssql/server:2022-latest` |
| SQLite | `sqlite` | File-based; no extra container |

The `DATABASE_TYPE` environment variable is read by the Aspire AppHost and controls:

1. Which database container is started alongside Chronicle
2. Which Chronicle projection **sink type** is injected into the Lending and Members backends

The sink type can also be overridden independently at any time via the `Cratis__Chronicle__DefaultSinkTypeId` environment variable on each backend process.

## 🚀 Running with Aspire (recommended)

Use the convenience bash scripts from the `Library/` folder:

```shell
# MongoDB (default)
./run-mongodb.sh

# PostgreSQL
./run-postgresql.sh

# Microsoft SQL Server
./run-mssql.sh

# SQLite
./run-sqlite.sh
```

Or set `DATABASE_TYPE` directly:

```shell
DATABASE_TYPE=postgresql dotnet run --project Composition/Composition.csproj
```

The Aspire dashboard opens automatically at [http://localhost:15888](http://localhost:15888) (no login token required).

> **⚠️ Local development only** — The docker-compose files embed hardcoded credentials (PostgreSQL password `chronicle`, SQL Server SA password `Chronicle_Str0ng!`, Vault root token `root`, Keycloak admin `admin`/`admin`). These are intentionally weak and must never be used in any environment beyond your local machine.

## 🐳 Running with docker compose

Pass the `--docker` flag to a script, or use docker compose profiles directly:

```shell
# MongoDB (default — profile can be omitted)
docker compose --profile mongodb -f docker-compose.yml up -d

# PostgreSQL
docker compose --profile postgresql -f docker-compose.yml up -d

# Microsoft SQL Server
docker compose --profile mssql -f docker-compose.yml up -d

# SQLite
docker compose --profile sqlite -f docker-compose.yml up -d
```

After the infrastructure is running, start the backends and frontends manually:

```shell
# Lending backend (port 5000)
cd Lending && dotnet run

# Members backend (port 5001)
cd Members && dotnet run

# Lending frontend (port 9000)
cd Lending && yarn dev

# Members frontend (port 9001)
cd Members && yarn dev
```

## 🔗 Service URLs

### Aspire

| Service | URL |
|---------|-----|
| Lending app (via AuthProxy) | [http://localhost:7000](http://localhost:7000) |
| Members app (via AuthProxy) | [http://localhost:7001](http://localhost:7001) |
| Lending backend Swagger | [http://localhost:5000/swagger](http://localhost:5000/swagger) |
| Members backend Swagger | [http://localhost:5001/swagger](http://localhost:5001/swagger) |
| Lending frontend (direct) | [http://localhost:9000](http://localhost:9000) |
| Members frontend (direct) | [http://localhost:9001](http://localhost:9001) |
| Chronicle Workbench | [http://localhost:8080](http://localhost:8080) |
| HashiCorp Vault | [http://localhost:8200](http://localhost:8200) |
| Keycloak – Lending | [http://localhost:8090](http://localhost:8090) |
| Keycloak – Members | [http://localhost:8091](http://localhost:8091) |
| Aspire Dashboard | [http://localhost:15888](http://localhost:15888) |

> **Note:** The backend root (`/`) returns 404 because no static files are built in Aspire dev mode — the frontend is served by the Vite dev servers at 9000/9001. Use the `/swagger` path to verify the backend is alive, or access the full app via AuthProxy.

### docker compose

| Service | URL |
|---------|-----|
| Lending backend | [http://localhost:5000](http://localhost:5000) |
| Members backend | [http://localhost:5001](http://localhost:5001) |
| Lending frontend | [http://localhost:9000](http://localhost:9000) |
| Members frontend | [http://localhost:9001](http://localhost:9001) |
| Chronicle Workbench | [http://localhost:8080](http://localhost:8080) |
| HashiCorp Vault | [http://localhost:8200](http://localhost:8200) |
| Keycloak – Lending | [http://localhost:8090](http://localhost:8090) |
| Keycloak – Members | [http://localhost:8091](http://localhost:8091) |

## 🔍 Inspecting the Data

### Chronicle Workbench

The Chronicle Workbench at [http://localhost:8080](http://localhost:8080) lets you browse the event log, observe projections, and inspect read models directly — regardless of which database backend is running. This is the primary tool for understanding what Chronicle has stored.

### HashiCorp Vault

Vault stores the compliance encryption keys that Chronicle uses to encrypt personal data fields. In dev mode it runs with a fixed root token of `root`.

**Web UI** — open [http://localhost:8200/ui](http://localhost:8200/ui), choose **Token** authentication, and enter `root`.

Navigate to **Secrets → secret** to browse the KV v2 store. Chronicle writes keys under:

```text
secret/<event-store>/<namespace>/<identifier>/<revision>
```

For example, a key for the default event store and namespace would appear at:

```text
secret/system/default/<encryption-key-id>/1
```

**CLI** — set the Vault address and token, then list or read secrets:

```shell
export VAULT_ADDR=http://localhost:8200
export VAULT_TOKEN=root

# List all secret paths (top-level)
vault kv list secret/

# List keys for a specific event store and namespace
vault kv list secret/system/default/

# Read a specific key
vault kv get secret/system/default/<identifier>/1
```

### MongoDB

Applies when `DATABASE_TYPE=mongodb` (the default).

MongoDB runs on port 27017 in both Aspire and docker-compose. In docker-compose mode it is embedded inside the Chronicle development image; in Aspire mode it runs as a separate managed container.

**Connection string:** `mongodb://localhost:27017`

Recommended tools:

- [MongoDB Compass](https://www.mongodb.com/products/compass) — GUI, connect with the connection string above
- `mongosh` CLI:

```shell
mongosh mongodb://localhost:27017

# Chronicle stores event logs in the chronicle-db database
use chronicle-db

# List collections
show collections

# Inspect event log entries
db.event_log.find().limit(10)
```

### PostgreSQL

Applies when `DATABASE_TYPE=postgresql`.

| Setting | Value |
|---------|-------|
| Host | `localhost` |
| Port | `5432` |
| Database | `chronicle` |
| Username | `chronicle` |
| Password | `chronicle` |

Recommended tools:

- [pgAdmin](https://www.pgadmin.org) — full GUI, register a server with the credentials above
- [TablePlus](https://tableplus.com) / [DBeaver](https://dbeaver.io) — cross-database GUI clients
- `psql` CLI:

```shell
psql -h localhost -U chronicle -d chronicle
```

```shell
# List all tables
\dt

# Show the event log table
SELECT * FROM event_log LIMIT 10;

# Count events by event store
SELECT event_store_id, COUNT(*) FROM event_log GROUP BY event_store_id;
```

### Microsoft SQL Server

Applies when `DATABASE_TYPE=mssql`.

| Setting | Value |
|---------|-------|
| Host | `localhost` |
| Port | `1433` |
| Database | `chronicle` |
| Username | `sa` |
| Password | `Chronicle_Str0ng!` |

Recommended tools:

- [Azure Data Studio](https://azure.microsoft.com/products/data-studio) — free, cross-platform GUI
- [DBeaver](https://dbeaver.io) — cross-database GUI client
- `sqlcmd` CLI:

```shell
sqlcmd -S "localhost,1433" -U sa -P "Chronicle_Str0ng!" -d chronicle
```

```sql
-- List tables
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE';
GO

-- Sample event log rows
SELECT TOP 10 * FROM event_log;
GO
```

### SQLite

Applies when `DATABASE_TYPE=sqlite`. The database file lives inside the Chronicle container at `/data/chronicle.db`. It is persisted in a named Docker volume (`chronicle-sqlite-data`).

**Copy the file out of the running container:**

```shell
# Find the container name (look for chronicle)
docker ps --format '{{.Names}}' | grep chronicle

# Copy the database file to the current directory
docker cp <container-name>:/data/chronicle.db ./chronicle.db
```

Or open it directly with an interactive shell:

```shell
docker exec -it <container-name> sh
sqlite3 /data/chronicle.db
```

```sql
-- List tables
.tables

-- Sample event log rows
SELECT * FROM event_log LIMIT 10;
```

Recommended desktop tool: [DB Browser for SQLite](https://sqlitebrowser.org) — open the copied `chronicle.db` file directly.

## 🧪 Running specifications

```shell
# From the Library root
dotnet test Library.slnx
```

## 🏘️ Cratis Stack

| Project | Description |
|---------|-------------|
| [Specifications](https://github.com/cratis/specifications) | BDD Specification by Example for xUnit |
| [Application Model (Arc)](https://github.com/cratis/applicationmodel) | Cratis application model |
| [Chronicle](https://github.com/cratis/chronicle) | Event sourcing kernel |
| [AuthProxy](https://github.com/cratis/authproxy) | Authentication reverse proxy |

For more details see [cratis.io](https://cratis.io).
