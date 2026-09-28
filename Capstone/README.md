# Full-stack author registration

A small Cratis library slice based on the MongoDB choice of `dotnet new cratis`. `Authors/` keeps the event-source identity, command and event, observable read model, and React screen together. The [full-stack capstone](https://cratis.io/build-a-full-app/) walks through these exact files. The rest of the generated host is in `Program.cs`, `App.tsx`, and `.frontend/`. The Samples repository pins .NET dependencies in `Directory.Packages.props` and frontend dependencies in this sample's `package-lock.json`.

## Run locally

Prerequisites: .NET 10 SDK, Node.js 23 or newer, npm, Docker with Compose. From this directory:

```sh
docker compose up -d
dotnet build Capstone.csproj --configuration Debug
npm ci
npm run build
```

The Debug build generates `Authors/RegisterAuthor.ts`, `Authors/Author.ts`, and `Authors/index.ts` from the C# slice; do not edit the proxies. In separate terminals, run `dotnet run --no-launch-profile` and `npm run dev`, then visit http://localhost:9000. Click **Add author**, enter a name, and submit it. The new name should appear in the list after Chronicle processes `AuthorRegistered` into the MongoDB read model. The template's Chronicle and MongoDB development settings are in `appsettings.json`.

To start over with empty local sample data, stop the app and run `docker compose down -v` **from this directory**. This deletes the Compose project's container data; do not run it against other projects. The template's development credentials and exposed ports are not production settings. This sample does not model other library operations, authentication, or deployment. The CI build checks compilation and proxy generation, not a live Chronicle/MongoDB interaction.
