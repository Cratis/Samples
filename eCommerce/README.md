# 🛍️ eCommerce - Understanding Event Sourcing

This sample is based on the book *Understanding Event Sourcing* leveraging the entire Cratis Stack.
Curious to learn how this sample got started, it was originally live streamed on YouTube and can
be found [here](https://www.youtube.com/live/WvBB3WnMIxM).

## 📖 About the book

*Understanding Event Sourcing* by Martin Dilger walks you through what Event Sourcing is and how you
can model systems using the Event Modelling technique.

- 𝌭 [Learn about Event Modelling from the source](https://eventmodeling.org/posts/what-is-event-modeling/)
- 🧩 [The event model used as basis for this ](https://miro.com/app/board/uXjVKvTN_NQ=/)
- 📗 [Get the book](https://leanpub.com/eventmodeling-and-eventsourcing)
- 💻 [Original repository from the book](https://github.com/dilgerma/eventsourcing-book)

## 📚 Cratis Stack

The eCommerce sample leverages the entire Cratis Stack:

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

You'll find next to this README file the [.NET solution file](./eCommerce.sln).
Within it you'll find 2 C# projects; **eCommerce** and **eCommerce.Specs**, if you're using VSCode or other
editor/IDE that shows you the folder structure, these are located in [Implementation](./Implementation/) and
[Specifications](./Specifications/).

## 🍕 Vertical slices

The project has been setup with a vertical slices type of structure which also includes frontend
code. This means that you'll find both C#, TypeScript and React code sitting side by side.

Its organized around the different tasks or workflows that can be done, for instance for Carts you would have
**Add Item**, **Contents**, **Remove Item**. You'll see these as folders and within each of these folders
you'll find everything that is relevant for that folder. This gives you a very high cohesive solution
that is easy to navigate and collaborate around.

## 🚀 Running it

To get started, the sample is setup with Chronicle running as a container, this is configured using
[a docker compose file](./docker-compose.yml).

Navigate to the [Implementation](./Implementation/) folder and run the following in your terminal:

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

From the [Specifications](./Specifications/) folder you can run the specifications by
running `dotnet test`. In your IDE/Editor of choice, you might have a test explorer that will
automatically populate with the available **specifications**.
