// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;

namespace Quickstart.Common;

#region Snippet:Quickstart-Events
[EventType]
public record UserOnboarded(string Name, string Email);

[EventType]
public record BookAddedToInventory(string Title, string Author, string ISBN);

[EventType]
public record BookBorrowed(Guid UserId);
#endregion Snippet:Quickstart-Events

#region Snippet:Quickstart-Projection-Events
[EventType]
public record BookReturned();

[EventType]
public record BookOverdue();

[EventType]
public record BookReservationPlaced(Guid UserId);

[EventType]
public record BookReservationCancelled();

[EventType]
public record BookReservationFulfilled();
#endregion Snippet:Quickstart-Projection-Events
