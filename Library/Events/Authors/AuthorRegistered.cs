// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Authors;

namespace Events.Authors;

[EventType]
public record AuthorRegistered(AuthorName Name);
