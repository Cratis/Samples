using Concepts.Authors;
using Concepts.Books;

namespace Events.Books;

[EventType]
public record BookAddedToCatalog(ISBN ISBN, BootTitle Title, AuthorId Author, PublishedDate PublishedDate);
