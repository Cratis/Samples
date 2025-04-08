// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Library.Common;

namespace Library.Lenders.Listing;

public record Lender(
    FirstName FirstName,
    LastName LastName,
    Address Address,
    PostalCode PostalCode,
    City City,
    SocialSecurityNumber SocialSecurityNumber);
