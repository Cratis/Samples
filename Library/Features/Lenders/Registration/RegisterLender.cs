// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.EventSequences;
using Library.Common;

namespace Library.Lenders.Registration;

public record RegisterLender(FirstName FirstName, LastName LastName, Address Address, PostalCode PostalCode, City City, SocialSecurityNumber SocialSecurityNumber);

[Route("/api/lenders")]
public class RegisterLenderHandler(IEventLog eventLog) : ControllerBase
{
    [HttpPost("register")]
    public Task RegisterLender([FromBody] RegisterLender command)
    {
        eventLog.Append(
            LenderId.New(),
            new LenderRegistered(
                command.FirstName,
                command.LastName,
                command.Address,
                command.PostalCode,
                command.City,
                command.SocialSecurityNumber));

        return Task.CompletedTask;
    }
}
