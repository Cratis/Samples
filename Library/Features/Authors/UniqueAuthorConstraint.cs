// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events.Constraints;
using Library.Authors.Registration;

namespace Library.Authors;

public class UniqueAuthorConstraint : IConstraint
{
    public void Define(IConstraintBuilder builder) => builder
        .Unique(_ => _
            .On<AuthorRegistered>(@event => @event.Name));
}
