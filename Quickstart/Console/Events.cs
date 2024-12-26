using Cratis.Chronicle.Events;

namespace Quickstart;

#region Sample-Quickstart-Events
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
#endregion Sample-Quickstart-Events