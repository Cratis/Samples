using Cratis.Chronicle.Events;

namespace Quickstart.Common;

#region Snippet:Quickstart-Events
[EventType]
public record UserOnboarded(string Name, string Email);

[EventType]
public record BookAddedToInventory(string Title, string Author, string ISBN);

[EventType]
public record BookBorrowed(Guid UserId);

[EventType]
public record BookReturned();

[EventType]
public record BookOverdue();

[EventType]
public record ReservationPlaced(Guid UserId);

[EventType]
public record ReservationCancelled();

[EventType]
public record ReservationFulfilled();
#endregion Snippet:Quickstart-Events
