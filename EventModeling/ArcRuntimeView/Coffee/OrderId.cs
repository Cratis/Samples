// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;

namespace ArcRuntimeView.Coffee;

/// <summary>
/// Identifies one coffee order's event stream.
/// </summary>
/// <param name="Value">The order identifier.</param>
public record OrderId(Guid Value) : EventSourceId<Guid>(Value);
