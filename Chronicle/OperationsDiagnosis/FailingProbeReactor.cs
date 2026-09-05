// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Reactors;

namespace OperationsDiagnosis;

/// <summary>
/// Provides a deliberately failing observer for operations diagnosis.
/// </summary>
[Reactor]
public class FailingProbeReactor : IReactor
{
    /// <summary>
    /// Fails with a stable error so the event source becomes a failed observer partition.
    /// </summary>
    /// <param name="event">The requested probe.</param>
    /// <exception cref="ProbeConfiguredToFail">Always thrown for the fixture event.</exception>
    public void Requested(ProbeRequested @event) => throw new ProbeConfiguredToFail(@event.Name);
}

/// <summary>
/// The exception thrown by the deliberately failing probe.
/// </summary>
/// <param name="probeName">The probe configured to fail.</param>
public sealed class ProbeConfiguredToFail(ProbeName probeName)
    : Exception($"OD-001: Probe '{probeName.Value}' is configured to fail for diagnosis.");
