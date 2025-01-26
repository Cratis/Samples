// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#region Snippet:Quickstart-OverdueBooksProjection
using Cratis.Chronicle.Projections;

namespace Quickstart.Common;

public class OverdueBooksProjection : IProjectionFor<OverdueBook>
{
    public void Define(IProjectionBuilderFor<OverdueBook> builder) => builder
        .From<BookOverdue>(from => from.Set(m => m.Id).ToEventContextProperty(c => c.EventSourceId))
        .Join<BookBorrowed>(bookBuilder => bookBuilder
            .On(m => m.Id)
            .Set(m => m.UserId).To(e => e.UserId)
            .Set(m => m.Borrowed).ToEventContextProperty(c => c.Occurred))
        .Join<BookAddedToInventory>(bookBuilder => bookBuilder
            .On(m => m.Id)
            .Set(m => m.Title).To(e => e.Title))
        .Join<UserOnboarded>(userBuilder => userBuilder
            .On(m => m.UserId)
            .Set(m => m.User).To(e => e.Name))
        .RemovedWith<BookReturned>();
}
#endregion Snippet:Quickstart-OverdueBooksProjection
