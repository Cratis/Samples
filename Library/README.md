# 📚 Library

This sample shows how a Library system could be built using the entire Cratis Stack.
The intention of the sample is to show the entire Cratis Stack at work and how you set up an
application that has both a frontend and a backend and how we see that fitting together.

It also shows how we think about structure and "Vertical Slices" - in fact, more like just "Slices".

## 📚 Cratis Stack

The sample leverages the entire Cratis Stack:

| Project | Description |
| ------- | ----------- |
| [Specifications](https://github.com/cratis/specifictions) | Thin wrapper for xUnit/NUnit for writing BDD Specification by Example type of specs |
| [Application Model](https://github.com/cratis/applicationmodel) | The Cratis application model |
| [Chronicle](https://github.com/cratis/chronicle) | Chronicle for event sourcing |

For more details on how to leverage the stack, you can go [here](https://cratis.io).

## 📋 Pre-requisites

To be able to run the code, you'll need the following:

- [.NET 9](https://dot.net)
- [NodeJS](https://nodejs.org)
- [Docker](https://www.docker.com/products/docker-desktop/) or compatible container engine.
- Optional: [MongoDB Compass](https://www.mongodb.com/products/tools/compass) or preferred tooling for managing MongoDB.

## 🏘️ Projects

You'll find next to this README file the [.NET solution file](./Library.sln).
Within it you'll find 1 C# projects; **Library**, if you're using VSCode or other.

## 🍕 Vertical slices

The project has been setup with a vertical slices type of structure which also includes frontend
code. This means that you'll find both C#, TypeScript and React code sitting side by side.

It is organized around the different tasks or workflows that can be done. It all starts within the **Features** folder were you'll
find a specific feature called **Authors** for instance. Within each of these you'll find concrete slices, such as
**Registration**, **Listing**. You'll see these as folders and within each of these folders
you'll find everything that is relevant for that folder. This gives you a very high cohesive solution
that is easy to navigate and collaborate around. When we say everything, me mean everything; backend, frontend, automated specs (tests).

## 🚀 Running it

To get started, the sample is setup with Chronicle running as a container, this is configured using
[a docker compose file](./docker-compose.yml).

```shell
docker compose up -d
```

Once it is running you start the backend by running the following:

```shell
dotnet run
```

Or if you prefer, you can always use the watch functionality with hot reloading:

```shell
dotnet watch run
```

> Note: Some Chronicle artifacts require you to do restart the program since it won't hot-reload
> and register with the Chronicle server. By pressing **ctrl+r** in the window you're running
> the watcher, *dotnet* will rebuild and restart.

For the frontend you'll need to restore all dependencies first by running:

```shell
yarn
```

Once that is done you start the development server by running the following:

```shell
yarn dev
```

> Note: The Vite development server is set to proxy backend requests back onto the backend port.

Once everything is running, the backend is exposed on http://localhost:5000 and the frontend
on http://localhost:9000.

Cratis Chronicle Workbench should be available on: http://localhost:8080

## 🧪 Running specifications

From the project folder you can run the specifications by
running `dotnet test`. In your IDE/Editor of choice, you might have a test explorer that will
automatically populate with the available **specifications**.
