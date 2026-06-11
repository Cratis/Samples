// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
using Cratis.Arc.Identity;

namespace Members.Development;

public class Users : ICanProvideUsers
{
    public Task<IEnumerable<User>> Provide() => Task.FromResult<IEnumerable<User>>(
    [
        CreateUser("b2000000-0000-0000-0000-000000000001", "Alice Johnson", "alice@library.example.com"),
        CreateUser("b2000000-0000-0000-0000-000000000002", "Bob Smith", "bob@library.example.com"),
    ]);

    static User CreateUser(string userId, string displayName, string email) =>
        new(
            new ClientPrincipal
            {
                IdentityProvider = "aad",
                UserId = userId,
                UserDetails = email,
                UserRoles = ["anonymous", "authenticated"],
                Claims =
                [
                    new ClientPrincipalClaim { typ = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", val = userId },
                    new ClientPrincipalClaim { typ = "name", val = displayName },
                    new ClientPrincipalClaim { typ = "preferred_username", val = email },
                ],
            },
            new { Email = email });
}
#endif
