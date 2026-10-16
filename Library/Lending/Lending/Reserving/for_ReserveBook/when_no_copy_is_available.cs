// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Library.Lending.Reserving.for_ReserveBook;

public class when_no_copy_is_available : Specification
{
    static readonly ISBN _isbn = new("978-0316389704");
    readonly CommandScenario<ReserveBook> _scenario = new();
    CommandResult _result;

    async Task Because() => _result = await _scenario.Execute(new ReserveBook(_isbn, MemberId.New()));

    void Destroy() => _scenario.Dispose();

    [Fact] void should_not_succeed() => _result.ShouldNotBeSuccessful();
    [Fact] void should_have_validation_errors() => _result.ShouldHaveValidationErrors();
    [Fact] void should_append_nothing() => _scenario.AppendedEvents.Count.ShouldEqual(0);
}
