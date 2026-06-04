# 📚 Library Sample

This sample shows a full Library system built on the entire Cratis Stack across two independent microservices — **Lending** and **Members** — composed via an Aspire AppHost with the complete local infrastructure.

## 🏛️ Architecture

```
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
- [.NET Aspire workload](https://learn.microsoft.com/dotnet/aspire/fundamentals/setup-tooling): `dotnet workload install aspire`

## 🗄️ Database Backends

Chronicle supports multiple storage backends. The Composition AppHost and docker-compose files both support the same set of profiles. **MongoDB is the default** when no database type is specified.

| Database | `DATABASE_TYPE` value | Notes |
|----------|----------------------|-------|
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

When the AppHost is running, the Aspire dashboard is available at **http://localhost:15888**.

## 🐳 Running with docker compose

Pass the `--docker` flag to a script, or use docker compose profiles directly:

```shell
# MongoDB (default — profile can be omitted)
docker compose --profile mongodb -f Composition/docker-compose.yml up -d

# PostgreSQL
docker compose --profile postgresql -f Composition/docker-compose.yml up -d

# Microsoft SQL Server
docker compose --profile mssql -f Composition/docker-compose.yml up -d

# SQLite
docker compose --profile sqlite -f Composition/docker-compose.yml up -d
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

| Service | URL |
|---------|-----|
| Lending (via AuthProxy) | http://localhost:5000 (backend) / http://localhost:9000 (frontend) |
| Members (via AuthProxy) | http://localhost:5001 (backend) / http://localhost:9001 (frontend) |
| Chronicle Workbench | http://localhost:8080 |
| HashiCorp Vault | http://localhost:8200 |
| Keycloak – Lending | http://localhost:8090 |
| Keycloak – Members | http://localhost:8091 |
| Aspire Dashboard | http://localhost:15888 |

### Demo users

**Lending realm** (http://localhost:8090/realms/lending)

| Username | Password | Role |
|----------|----------|------|
| `librarian` | `librarian` | Librarian |
| `borrower` | `borrower` | Borrower |

**Members realm** (http://localhost:8091/realms/members)

| Username | Password |
|----------|----------|
| `alice` | `alice` |
| `bob` | `bob` |

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
