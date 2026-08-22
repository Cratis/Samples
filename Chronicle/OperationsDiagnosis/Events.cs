// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;

namespace OperationsDiagnosis;

/// <summary>
/// The event that records a request to run an operations probe.
/// </summary>
/// <param name="Name">The name of the requested probe.</param>
[EventType]
public record ProbeRequested(ProbeName Name);
