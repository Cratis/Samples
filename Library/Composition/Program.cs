// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.AuthProxy.Aspire;

var builder = DistributedApplication.CreateBuilder(args);

// HashiCorp Vault running in dev mode with a fixed root token for local development
var vault = builder.AddContainer("vault", "hashicorp/vault")
    .WithEnvironment("VAULT_DEV_ROOT_TOKEN_ID", "root")
    .WithEnvironment("VAULT_DEV_LISTEN_ADDRESS", "0.0.0.0:8200")
    .WithArgs("server", "-dev")
    .WithHttpEndpoint(targetPort: 8200, name: "http");

// Chronicle running in development mode (embedded MongoDB)
// Configured to use Vault for compliance encryption key storage
var chronicle = builder.AddCratisChronicle("chronicle")
    .WithEnvironment(
        "Cratis__Chronicle__Compliance__KeyStore__Vault__Address",
        vault.GetEndpoint("http"))
    .WithEnvironment("Cratis__Chronicle__Compliance__KeyStore__Vault__Token", "root")
    .WaitFor(vault);

// Keycloak for Lending - pre-seeded with librarian and borrower users
var keycloakLending = builder.AddContainer("keycloak-lending", "quay.io/keycloak/keycloak")
    .WithArgs("start-dev", "--import-realm")
    .WithBindMount("./keycloak/lending", "/opt/keycloak/data/import", isReadOnly: true)
    .WithEnvironment("KEYCLOAK_ADMIN", "admin")
    .WithEnvironment("KEYCLOAK_ADMIN_PASSWORD", "admin")
    .WithHttpEndpoint(targetPort: 8080, name: "http");

// Keycloak for Members - pre-seeded with member users
var keycloakMembers = builder.AddContainer("keycloak-members", "quay.io/keycloak/keycloak")
    .WithArgs("start-dev", "--import-realm")
    .WithBindMount("./keycloak/members", "/opt/keycloak/data/import", isReadOnly: true)
    .WithEnvironment("KEYCLOAK_ADMIN", "admin")
    .WithEnvironment("KEYCLOAK_ADMIN_PASSWORD", "admin")
    .WithHttpEndpoint(targetPort: 8080, name: "http");

// Lending backend - connected to Chronicle
var lending = builder.AddProject<Projects.Lending>("lending")
    .WithReference(chronicle)
    .WaitFor(chronicle);

// Members backend - connected to Chronicle
var members = builder.AddProject<Projects.Members>("members")
    .WithReference(chronicle)
    .WaitFor(chronicle);

// Lending frontend (Vite dev server via yarn, port 9000)
var lendingFrontend = builder.AddViteApp("lending-frontend", "../Lending")
    .WithYarn()
    .WithHttpEndpoint(targetPort: 9000, name: "http");

// Members frontend (Vite dev server via yarn, port 9001)
var membersFrontend = builder.AddViteApp("members-frontend", "../Members")
    .WithYarn()
    .WithHttpEndpoint(targetPort: 9001, name: "http");

var keycloakLendingEndpoint = keycloakLending.GetEndpoint("http");
var keycloakMembersEndpoint = keycloakMembers.GetEndpoint("http");

// AuthProxy for Lending - authenticates users via Keycloak and proxies to Lending backend/frontend.
// WithOidcProvider only accepts a static string for authority; we override it with a
// WithEnvironment callback so Aspire resolves the dynamic Keycloak endpoint at startup.
builder.AddAuthProxy("authproxy-lending")
    .WithHttpEndpoint(targetPort: 8080, name: "http")
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
    .WithHttpEndpoint(targetPort: 8080, name: "http")
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

