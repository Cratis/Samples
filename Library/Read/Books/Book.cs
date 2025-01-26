// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Authors;
using Concepts.Books;

namespace Read.Books;

public record Book(BookId Id, ISBN ISBN, BootTitle Title, AuthorName Author, PublishedDate PublishedDate);
