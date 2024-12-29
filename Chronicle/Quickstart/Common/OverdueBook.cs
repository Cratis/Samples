namespace Quickstart.Common;

public record OverdueBook(Guid Id, string Title, Guid UserId, string User, DateTimeOffset Borrowed);
