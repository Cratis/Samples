// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aspire.Hosting.ApplicationModel;
using Cratis.AuthProxy.Aspire;
using Cratis.Chronicle.Aspire;

var builder = DistributedApplication.CreateBuilder(args);

// Select the database backend. Accepted values (case-insensitive):
//   mongodb (default), postgresql, mssql, sqlite
// Override at runtime:
//   DATABASE_TYPE=postgresql dotnet run --project Library/Composition
var databaseType = (builder.Configuration["DATABASE_TYPE"] ?? "mongodb").ToLowerInvariant();

// Resolve which Chronicle projection sink type id to pass to the microservices.
// MongoDB sink:  22202c41-2be1-4547-9c00-f0b1f797fd75 (WellKnownSinkTypes.MongoDB)
// SQL sink:      f7d3a1e2-4b5c-4d6e-8f9a-0b1c2d3e4f5a (WellKnownSinkTypes.SQL)
const string mongoDbSinkTypeId = "22202c41-2be1-4547-9c00-f0b1f797fd75";
const string sqlSinkTypeId = "f7d3a1e2-4b5c-4d6e-8f9a-0b1c2d3e4f5a";
var sinkTypeId = string.Equals(databaseType, "postgresql", StringComparison.Ordinal)
                 || string.Equals(databaseType, "mssql", StringComparison.Ordinal)
                 || string.Equals(databaseType, "sqlite", StringComparison.Ordinal)
    ? sqlSinkTypeId
    : mongoDbSinkTypeId;

// HashiCorp Vault running in dev mode with a fixed root token for local development
var vault = builder.AddContainer("vault", "hashicorp/vault")
    .WithEnvironment("VAULT_DEV_ROOT_TOKEN_ID", "root")
    .WithEnvironment("VAULT_DEV_LISTEN_ADDRESS", "0.0.0.0:8200")
    .WithArgs("server", "-dev")
    .WithHttpEndpoint(port: 8200, targetPort: 8200, name: "http");

// Chronicle — storage backend is selected dynamically based on DATABASE_TYPE.
// When no configure callback is supplied AddCratisChronicle uses the development image (embedded MongoDB).
// For any explicit database choice we supply a callback to switch to the production image.
IResourceBuilder<ChronicleResource> chronicle;
if (string.Equals(databaseType, "postgresql", StringComparison.Ordinal))
{
    var db = builder.AddPostgres("postgres").AddDatabase("chronicle-db");
    chronicle = builder.AddCratisChronicle("chronicle", c => c.WithPostgreSql(db));
}
else if (string.Equals(databaseType, "mssql", StringComparison.Ordinal))
{
    var db = builder.AddSqlServer("mssql").AddDatabase("chronicle-db");
    chronicle = builder.AddCratisChronicle("chronicle", c => c.WithMsSql(db));
}
else if (string.Equals(databaseType, "sqlite", StringComparison.Ordinal))
{
    chronicle = builder.AddCratisChronicle("chronicle", c => c.WithSqlite("Data Source=/data/chronicle.db"));
}
else
{
    // mongodb — the latest-development image no longer bundles MongoDB, so spin up a dedicated container
    var mongo = builder.AddMongoDB("mongodb").AddDatabase("chronicle-db");
    chronicle = builder.AddCratisChronicle("chronicle", c => c.WithMongoDB(mongo));
}

// Wire Vault compliance key storage into Chronicle and pin to stable host ports.
// Management (workbench) runs on 8080, gRPC on 35000 — same as the container target ports.
chronicle
    .WithEnvironment(
        "Cratis__Chronicle__Compliance__KeyStore__Vault__Address",
        vault.GetEndpoint("http"))
    .WithEnvironment("Cratis__Chronicle__Compliance__KeyStore__Vault__Token", "root")
    .WaitFor(vault)
    .WithEndpoint("management", endpoint => endpoint.Port = 8080)
    .WithEndpoint("grpc", endpoint => endpoint.Port = 35000);

// Keycloak for Lending - pre-seeded with librarian and borrower users
var keycloakLending = builder.AddContainer("keycloak-lending", "quay.io/keycloak/keycloak")
    .WithArgs("start-dev", "--import-realm")
    .WithBindMount("./keycloak/lending", "/opt/keycloak/data/import", isReadOnly: true)
    .WithEnvironment("KEYCLOAK_ADMIN", "admin")
    .WithEnvironment("KEYCLOAK_ADMIN_PASSWORD", "admin")
    .WithHttpEndpoint(port: 8090, targetPort: 8080, name: "http");

// Keycloak for Members - pre-seeded with member users
var keycloakMembers = builder.AddContainer("keycloak-members", "quay.io/keycloak/keycloak")
    .WithArgs("start-dev", "--import-realm")
    .WithBindMount("./keycloak/members", "/opt/keycloak/data/import", isReadOnly: true)
    .WithEnvironment("KEYCLOAK_ADMIN", "admin")
    .WithEnvironment("KEYCLOAK_ADMIN_PASSWORD", "admin")
    .WithHttpEndpoint(port: 8091, targetPort: 8080, name: "http");

// Lending backend - connected to Chronicle; projection sink type flows from DATABASE_TYPE
var lending = builder.AddProject<Projects.Lending>("lending")
    .WithReference(chronicle)
    .WithEnvironment("Cratis__Chronicle__DefaultSinkTypeId", sinkTypeId)
    .WaitFor(chronicle);

// Members backend - connected to Chronicle; projection sink type flows from DATABASE_TYPE
var members = builder.AddProject<Projects.Members>("members")
    .WithReference(chronicle)
    .WithEnvironment("Cratis__Chronicle__DefaultSinkTypeId", sinkTypeId)
    .WaitFor(chronicle);

// Lending frontend (Vite dev server via yarn, port 9000)
var lendingFrontend = builder.AddViteApp("lending-frontend", "../Lending")
    .WithYarn()
    .WithHttpEndpoint(port: 9000, targetPort: 9000, name: "http", isProxied: false);

// Members frontend (Vite dev server via yarn, port 9001)
var membersFrontend = builder.AddViteApp("members-frontend", "../Members")
    .WithYarn()
    .WithHttpEndpoint(port: 9001, targetPort: 9001, name: "http", isProxied: false);

var keycloakLendingEndpoint = keycloakLending.GetEndpoint("http");
var keycloakMembersEndpoint = keycloakMembers.GetEndpoint("http");

// AuthProxy for Lending - authenticates users via Keycloak and proxies to Lending backend/frontend.
// WithOidcProvider only accepts a static string for authority; we override it with a
// WithEnvironment callback so Aspire resolves the dynamic Keycloak endpoint at startup.
builder.AddAuthProxy("authproxy-lending")
    .WithHttpEndpoint(port: 7000, targetPort: 8080, name: "http")
    .WithBackend("main", lending)
    .WithFrontend("main", lendingFrontend)
    .WithOidcProvider(
        "Keycloak",
        OidcProviderType.Custom,
        authority: string.Empty,
        clientId: "lending-app",
        clientSecret: "lending-secret")
    .WithEnvironment(ctx =>
        ctx.EnvironmentVariables["Cratis__AuthProxy__Authentication__OidcProviders__0__Authority"] =
            ReferenceExpression.Create($"{keycloakLendingEndpoint}/realms/lending"))
    .WithSpecifiedTenantResolution("default")
    .WaitFor(keycloakLending)
    .WaitFor(lending)
    .WaitFor(lendingFrontend);

// AuthProxy for Members - authenticates users via Keycloak and proxies to Members backend/frontend.
// WithOidcProvider only accepts a static string for authority; we override it with a
// WithEnvironment callback so Aspire resolves the dynamic Keycloak endpoint at startup.
builder.AddAuthProxy("authproxy-members")
    .WithHttpEndpoint(port: 7001, targetPort: 8080, name: "http")
    .WithBackend("main", members)
    .WithFrontend("main", membersFrontend)
    .WithOidcProvider(
        "Keycloak",
        OidcProviderType.Custom,
        authority: string.Empty,
        clientId: "members-app",
        clientSecret: "members-secret")
    .WithEnvironment(ctx =>
        ctx.EnvironmentVariables["Cratis__AuthProxy__Authentication__OidcProviders__0__Authority"] =
            ReferenceExpression.Create($"{keycloakMembersEndpoint}/realms/members"))
    .WithSpecifiedTenantResolution("default")
    .WaitFor(keycloakMembers)
    .WaitFor(members)
    .WaitFor(membersFrontend);

await builder.Build().RunAsync();

