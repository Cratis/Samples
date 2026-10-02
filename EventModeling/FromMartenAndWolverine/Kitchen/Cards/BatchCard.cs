// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FromMartenAndWolverine.Kitchen.Cards;

/// <summary>
/// Shows volunteers the current preparation progress.
/// </summary>
public class BatchCard
{
    /// <summary>
    /// Gets or sets the batch identity.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the recipe name.
    /// </summary>
    public string Recipe { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the intended number of portions.
    /// </summary>
    public int TargetPortions { get; set; }

    /// <summary>
    /// Gets or sets the number of prepared portions.
    /// </summary>
    public int PreparedPortions { get; set; }

    /// <summary>
    /// Gets or sets whether the batch has reached the serving table.
    /// </summary>
    public bool Served { get; set; }
}
