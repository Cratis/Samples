// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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

// AuthProxy for Lending - authenticates users via Keycloak and proxies to Lending backend/frontend
builder.AddContainer("authproxy-lending", "cratis/authproxy")
    .WithHttpEndpoint(targetPort: 8080, name: "http")
    .WithEnvironment(
        "Cratis__AuthProxy__Authentication__OidcProviders__0__Name",
        "Keycloak")
    .WithEnvironment(
        "Cratis__AuthProxy__Authentication__OidcProviders__0__Type",
        "Custom")
    .WithEnvironment(
        "Cratis__AuthProxy__Authentication__OidcProviders__0__Authority",
        ReferenceExpression.Create($"{keycloakLending.GetEndpoint("http")}/realms/lending"))
    .WithEnvironment(
        "Cratis__AuthProxy__Authentication__OidcProviders__0__ClientId",
        "lending-app")
    .WithEnvironment(
        "Cratis__AuthProxy__Authentication__OidcProviders__0__ClientSecret",
        "lending-secret")
    .WithEnvironment(
        "Cratis__AuthProxy__Services__app__Backend__BaseUrl",
        lending.GetEndpoint("http"))
    .WithEnvironment(
        "Cratis__AuthProxy__Services__app__Frontend__BaseUrl",
        lendingFrontend.GetEndpoint("http"))
    .WaitFor(keycloakLending)
    .WaitFor(lending)
    .WaitFor(lendingFrontend);

// AuthProxy for Members - authenticates users via Keycloak and proxies to Members backend/frontend
builder.AddContainer("authproxy-members", "cratis/authproxy")
    .WithHttpEndpoint(targetPort: 8080, name: "http")
    .WithEnvironment(
        "Cratis__AuthProxy__Authentication__OidcProviders__0__Name",
        "Keycloak")
    .WithEnvironment(
        "Cratis__AuthProxy__Authentication__OidcProviders__0__Type",
        "Custom")
    .WithEnvironment(
        "Cratis__AuthProxy__Authentication__OidcProviders__0__Authority",
        ReferenceExpression.Create($"{keycloakMembers.GetEndpoint("http")}/realms/members"))
    .WithEnvironment(
        "Cratis__AuthProxy__Authentication__OidcProviders__0__ClientId",
        "members-app")
    .WithEnvironment(
        "Cratis__AuthProxy__Authentication__OidcProviders__0__ClientSecret",
        "members-secret")
    .WithEnvironment(
        "Cratis__AuthProxy__Services__app__Backend__BaseUrl",
        members.GetEndpoint("http"))
    .WithEnvironment(
        "Cratis__AuthProxy__Services__app__Frontend__BaseUrl",
        membersFrontend.GetEndpoint("http"))
    .WaitFor(keycloakMembers)
    .WaitFor(members)
    .WaitFor(membersFrontend);

await builder.Build().RunAsync();
