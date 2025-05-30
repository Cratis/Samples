// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle;
using Cratis.Chronicle.Concepts.Observation;
using Cratis.Chronicle.Events;
using Cratis.Chronicle.Grains.Observation;
using FailedPartition = Cratis.Chronicle.Contracts.Observation.FailedPartition;

namespace eCommerce.Specs;

/// <summary>
/// Helper methods for working with observers for integration testing purposes.
/// </summary>
public static class ObserverHelpers
{
    /// <summary>
    /// Wait for the observer to reach a specific running state.
    /// </summary>
    /// <param name="observer">Observer to wait for.</param>
    /// <param name="runningState">The expected <see cref="ObserverRunningState"/> to wait for.</param>
    /// <param name="timeout">Optional timeout. If none is provided, it will default to 5 seconds.</param>
    /// <returns>Awaitable task.</returns>
    public static async Task WaitForState(this IObserver observer, ObserverRunningState runningState, TimeSpan? timeout = default)
    {
        timeout ??= TimeSpanFactory.DefaultTimeout();
        var currentRunningState = ObserverRunningState.Unknown;
        using var cts = new CancellationTokenSource(timeout.Value);
        while (currentRunningState != runningState && !cts.IsCancellationRequested)
        {
            var state = await observer.GetState();
            currentRunningState = state.RunningState;
            await Task.Delay(20, cts.Token);
        }
    }

    /// <summary>
    /// Wait till the observer is active, with an optional timeout.
    /// </summary>
    /// <param name="observer">Observer to wait for.</param>
    /// <param name="timeout">Optional timeout. If none is provided, it will default to 5 seconds.</param>
    /// <returns>Awaitable task.</returns>
    public static async Task WaitTillActive(this IObserver observer, TimeSpan? timeout = default) =>
        await observer.WaitForState(ObserverRunningState.Active, timeout ?? TimeSpanFactory.DefaultTimeout());

    /// <summary>
    /// Wait till the observer has been subscribed, with an optional timeout.
    /// </summary>
    /// <param name="observer">Observer to wait for.</param>
    /// <param name="timeout">Optional timeout. If none is provided, it will default to 5 seconds.</param>
    /// <returns>Awaitable task.</returns>
    public static async Task WaitTillSubscribed(this IObserver observer, TimeSpan? timeout = default)
    {
        timeout ??= TimeSpanFactory.DefaultTimeout();
        using var cts = new CancellationTokenSource(timeout.Value);
        while (!await observer.IsSubscribed())
        {
            await Task.Delay(100, cts.Token);
        }
    }

    /// <summary>
    /// Wait till the observer reaches a specific event sequence number, with an optional timeout.
    /// </summary>
    /// <param name="observer">Observer to wait for.</param>
    /// <param name="eventSequenceNumber">The expected <see cref="EventSequenceNumber"/> to wait for.</param>
    /// <param name="timeout">Optional timeout. If none is provided, it will default to 5 seconds.</param>
    /// <returns>Awaitable task.</returns>
    public static async Task WaitTillReachesEventSequenceNumber(this IObserver observer, EventSequenceNumber eventSequenceNumber, TimeSpan? timeout = default)
    {
        timeout ??= TimeSpanFactory.DefaultTimeout();
        using var cts = new CancellationTokenSource(timeout.Value);
        while (true)
        {
            var state = await observer.GetState();
            if (state.LastHandledEventSequenceNumber.Value == eventSequenceNumber.Value)
            {
                break;
            }

            await Task.Delay(20, cts.Token);
        }
    }

    /// <summary>
    /// Wait for there to be failed partitions for the observer, with an optional timeout.
    /// </summary>
    /// <param name="eventStore"><see cref="IEventStore"/> service.</param>
    /// <param name="observerId">The <see cref="ObserverId"/> to wait for.</param>
    /// <param name="timeout">Optional timeout. If none is provided, it will default to 5 seconds.</param>
    /// <returns>Collection of <see cref="FailedPartition"/>.</returns>
    public static async Task<IEnumerable<FailedPartition>> WaitForThereToBeFailedPartitions(this IEventStore eventStore, ObserverId observerId, TimeSpan? timeout = default)
    {
        timeout ??= TimeSpanFactory.DefaultTimeout();
        var failedPartitions = Enumerable.Empty<FailedPartition>();
        using var cts = new CancellationTokenSource(timeout.Value);
        while (!failedPartitions.Any() && !cts.IsCancellationRequested)
        {
            failedPartitions = await eventStore.Connection.Services.FailedPartitions.GetFailedPartitions(new()
            {
                EventStore = eventStore.Name.Value,
                Namespace = EventStoreNamespaceName.Default,
                ObserverId = observerId
            });
            await Task.Delay(100, cts.Token);
        }

        return failedPartitions;
    }
}
