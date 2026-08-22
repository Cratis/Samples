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
/// The exception that is thrown when the operations diagnosis canary is observed.
/// </summary>
public sealed class ProbeConfiguredToFail : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProbeConfiguredToFail"/> class.
    /// </summary>
    public ProbeConfiguredToFail()
        : this(ProbeName.FailureFixture)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProbeConfiguredToFail"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public ProbeConfiguredToFail(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProbeConfiguredToFail"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The exception that caused this failure.</param>
    public ProbeConfiguredToFail(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProbeConfiguredToFail"/> class.
    /// </summary>
    /// <param name="probeName">The probe configured to fail.</param>
    public ProbeConfiguredToFail(ProbeName probeName)
        : base($"OD-001: Probe '{probeName.Value}' is configured to fail for diagnosis.")
    {
    }
}
