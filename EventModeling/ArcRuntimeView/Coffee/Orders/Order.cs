// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using ArcRuntimeView.Coffee.Ordering;
using ArcRuntimeView.Coffee.Preparing;
using Cratis.Arc.Queries.ModelBound;
using Cratis.Chronicle.Projections.ModelBound;
using MongoDB.Driver;

namespace ArcRuntimeView.Coffee.Orders;

/// <summary>
/// Displays the drink and its preparation time.
/// </summary>
/// <param name="Id">The order identifier.</param>
/// <param name="Drink">The requested drink.</param>
/// <param name="PreparedAt">The preparation time, or null while waiting.</param>
[ReadModel]
[FromEvent<OrderPlaced>]
[FromEvent<OrderPrepared>]
public record Order(OrderId Id, string Drink, DateTimeOffset? PreparedAt)
{
    /// <summary>
    /// Lists the orders on the counter.
    /// </summary>
    /// <param name="collection">The projected orders.</param>
    /// <returns>The current orders.</returns>
    public static async Task<IEnumerable<Order>> AllOrders(IMongoCollection<Order> collection) =>
        await collection.Find(_ => true).ToListAsync();
}
