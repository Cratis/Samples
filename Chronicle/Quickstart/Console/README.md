# Cratis Chronicle Console Quickstart

Prerequisites:

- [.NET 8 or higher](https://dot.net)
- [Docker Desktop or compatible](https://www.docker.com/products/docker-desktop/)
- MongoDB client (e.g. [MongoDB Compass](https://www.mongodb.com/products/tools/compass))

## Running the sample

From this folder, run `docker compose up -d` in a terminal.
Then run the project itself (`dotnet run`).

## Using the UI

The UI is built as an old DOS/Terminal type of application.
You can navigate using the keyboard with arrow keys and enter for actions.
In forms were you have to select multiple items you use the arrow keys to
select and then use **TAB** to switch between the different items that needs
to be selected. Hitting enter will then perform the action.

On the startup screen you will see at the top a menu item "Initialize demo data",
this will give you users and books to work with, hit enter on this to start with.

## Seeing data in the database

Using the MongoDB client you can simply open the database called `QuickStart`,
it will hold the resulting data.

## Structure

The code is found in the [Common](../Common/) folder and holds all events and
projections used in this sample.
