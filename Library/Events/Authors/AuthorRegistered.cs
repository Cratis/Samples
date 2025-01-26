using Concepts.Authors;

namespace Events.Authors;

[EventType]
public record AuthorRegistered(AuthorName Name);
