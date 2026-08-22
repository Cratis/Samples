// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle;
using Cratis.Chronicle.Registrations;

namespace Chronicle.Backend;

sealed class ChronicleReadiness(IEventStore eventStore)
{
    static readonly TimeSpan _registrationTimeout = TimeSpan.FromSeconds(5);

    public async Task<IResult?> GetUnavailableResult()
    {
        RegistrationOutcome outcome;

        try
        {
            outcome = await eventStore.WaitForRegistration(_registrationTimeout);
        }
        catch (TaskCanceledException)
        {
            return Results.Problem(
                "Client artifact registration did not finish before the request deadline.",
                statusCode: StatusCodes.Status503ServiceUnavailable,
                title: "Chronicle is not ready");
        }

        return outcome.IsSuccess
            ? null
            : Results.Problem(
                "Inspect the application and Chronicle logs before retrying the request.",
                statusCode: StatusCodes.Status503ServiceUnavailable,
                title: "Chronicle registration failed");
    }
}
