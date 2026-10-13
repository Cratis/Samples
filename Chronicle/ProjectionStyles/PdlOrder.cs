// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace ProjectionStyles;

/// <summary>
/// Defines the separately registered schema targeted by the Workbench PDL preview.
/// </summary>
/// <param name="Id">The order stream identity.</param>
/// <param name="CustomerName">The customer's display name.</param>
/// <param name="IsShipped">Whether a shipment event has occurred.</param>
/// <param name="ItemCount">Add events minus removal events; not the distinct active line count.</param>
/// <param name="LastUpdated">The occurrence time of the last contributing order event.</param>
/// <param name="Lines">Product-keyed lines with joined product names.</param>
public record PdlOrder(OrderId Id, CustomerName CustomerName, bool IsShipped, int ItemCount, DateTimeOffset LastUpdated, IEnumerable<OrderLine> Lines);
