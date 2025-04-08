// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Library.Books.Inventory;

[EventType]
public record StockChanged(StockCount StockCount);
