// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace ProjectionStyles;

/// <summary>
/// Represents an active line keyed by its product.
/// </summary>
/// <param name="ProductId">The product identifying the line.</param>
/// <param name="Quantity">The current quantity.</param>
/// <param name="ProductName">The joined product name; absent in the stream-local reducer.</param>
public record OrderLine(ProductId ProductId, int Quantity, ProductName? ProductName);
