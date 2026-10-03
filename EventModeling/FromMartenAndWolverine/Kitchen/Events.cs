// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FromMartenAndWolverine.Kitchen;

/// <summary>
/// Records a community kitchen's plan for one batch of soup.
/// </summary>
/// <param name="Recipe">The soup to prepare.</param>
/// <param name="TargetPortions">The intended number of portions.</param>
public record BatchPlanned(string Recipe, int TargetPortions);

/// <summary>
/// Records portions prepared for the serving table.
/// </summary>
/// <param name="Portions">The additional portions prepared.</param>
public record PortionsPrepared(int Portions);

/// <summary>
/// Records a batch reaching the serving table.
/// </summary>
/// <param name="Portions">The number of portions available to serve.</param>
public record BatchServed(int Portions);
