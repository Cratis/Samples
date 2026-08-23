// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;

namespace Chronicle.Backend;

/// <summary>
/// Records one immutable entry in a timeline's history.
/// </summary>
/// <param name="Text">The text captured in the timeline.</param>
[EventType]
public record TimelineEntryRecorded(TimelineEntryText Text);
