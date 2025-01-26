// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Authors;
using Concepts.Books;

namespace Events.Books;

[EventType]
public record BookAddedToCatalog(ISBN ISBN, BootTitle Title, AuthorId Author, PublishedDate PublishedDate);
