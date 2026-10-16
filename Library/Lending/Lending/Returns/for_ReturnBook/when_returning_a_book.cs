// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Library.Lending.Returns.for_ReturnBook;

public class when_returning_a_book : Specification
{
    static readonly ISBN _isbn = new("978-0316389704");
    readonly CommandScenario<ReturnBook> _scenario = new();
    CommandResult _result;

    async Task Because() => _result = await _scenario.Execute(new ReturnBook(_isbn, MemberId.New()));

    void Destroy() => _scenario.Dispose();

    [Fact] void should_succeed() => _result.ShouldBeSuccessful();
    [Fact] void should_append_the_return() => _scenario.AppendedEvents.Count.ShouldEqual(1);
}
