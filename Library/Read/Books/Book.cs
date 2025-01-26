using Concepts.Authors;
using Concepts.Books;

namespace Read.Books;

public record Book(BookId Id, ISBN ISBN, BootTitle Title, AuthorName Author, PublishedDate PublishedDate);
