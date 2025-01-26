using Concepts.Authors;
using Concepts.Books;

namespace Api.Books;

public record RegisterBook(ISBN ISBN, BootTitle Title, AuthorId Author, PublishedDate PublishedDate);
