// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Cratis.Arc.Commands;
using Cratis.Arc.Validation;
using Library.Inventory.Adding;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Lending.Borrowing.for_BorrowBook;

// CommandScenario has no severity option, so this executes through ICommandPipeline's allowedSeverity overload,
// using the scenario's own provider (the scenario exposes no accessor, hence the reflection).
public class when_no_copy_is_available_and_the_caller_allows_errors : Specification
{
    static readonly ISBN _isbn = new("978-0316389704");
    readonly CommandScenario<BorrowBook> _scenario = new();
    CommandResult _result;

    void Establish()
    {
        _scenario.Given.ForEventSource(_isbn).Events(
            new BookAddedToInventory("The Last Wish", AuthorId.New(), 1),
            new BookBorrowed(_isbn, "The Last Wish", MemberId.New(), DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(21)));
    }

    async Task Because()
    {
        var command = new BorrowBook(_isbn, MemberId.New());
        await _scenario.Validate(command);

        var provider = (IServiceProvider)typeof(CommandScenario<BorrowBook>)
            .GetField("_serviceProvider", BindingFlags.Instance | BindingFlags.NonPublic)!
            .GetValue(_scenario)!;
        var pipeline = provider.GetRequiredService<ICommandPipeline>();

        _result = await pipeline.Execute(command, provider, ValidationResultSeverity.Error);
    }

    void Destroy() => _scenario.Dispose();

    [Fact] void should_not_succeed() => _result.ShouldNotBeSuccessful();
    [Fact] void should_have_validation_errors() => _result.ShouldHaveValidationErrors();
    [Fact] void should_append_nothing() => _scenario.AppendedEvents.Count.ShouldEqual(0);
}
