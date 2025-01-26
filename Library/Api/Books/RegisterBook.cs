// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Authors;
using Concepts.Books;

namespace Api.Books;

public record RegisterBook(ISBN ISBN, BootTitle Title, AuthorId Author, PublishedDate PublishedDate);
