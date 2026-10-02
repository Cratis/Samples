// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FromMartenAndWolverine.Kitchen;

/// <summary>
/// Rebuilds one soup batch from its event stream.
/// </summary>
public class SoupBatch
{
    /// <summary>
    /// Gets or sets the stream identity assigned by Marten.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets the intended number of portions.
    /// </summary>
    public int TargetPortions { get; private set; }

    /// <summary>
    /// Gets the number of prepared portions.
    /// </summary>
    public int PreparedPortions { get; private set; }

    /// <summary>
    /// Gets whether the batch has reached the serving table.
    /// </summary>
    public bool Served { get; private set; }

    /// <summary>
    /// Applies the batch plan.
    /// </summary>
    /// <param name="@event">The recorded plan.</param>
    public void Apply(BatchPlanned @event) => TargetPortions = @event.TargetPortions;

    /// <summary>
    /// Applies a preparation fact.
    /// </summary>
    /// <param name="@event">The recorded portions.</param>
    public void Apply(PortionsPrepared @event) => PreparedPortions += @event.Portions;

    /// <summary>
    /// Marks the batch as served.
    /// </summary>
    /// <param name="@event">The serving fact.</param>
    public void Apply(BatchServed @event) => Served = true;
}
