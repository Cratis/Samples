#if DEBUG
// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Chronicle.Testing.Commands;
using Cratis.Arc.Commands;
using Cratis.Arc.Testing.Commands;
using Cratis.Chronicle.Events;
using Cratis.Specifications;
using Shop.Common;
using Xunit;

namespace Shop.Shop.Customers.IssueInvite.when_issuing_an_invite;

public class when_issuing_an_invite : global::Cratis.Specifications.Specification, global::System.IDisposable
{
    readonly global::Cratis.Arc.Testing.Commands.CommandScenario<global::Shop.Shop.Customers.IssueInvite.IssueInvite> _scenario = new();
    global::Cratis.Arc.Commands.CommandResult _result = null!;

    void Establish()
    {
        _scenario.PinGeneratedValue("inviteCode", new global::Shop.Common.InviteCode("K7MPQ2XRT9"));
    }

    async global::System.Threading.Tasks.Task Because() => _result = await _scenario.Execute(new global::Shop.Shop.Customers.IssueInvite.IssueInvite(new global::Shop.Common.CustomerId("cus_01m4t7txm0hrn5rwemy2dkwtgq")));

    [global::Xunit.FactAttribute] void should_succeed() => _result.ShouldBeSuccessful();
    [global::Xunit.FactAttribute] async global::System.Threading.Tasks.Task should_have_appended_invite_issued() => await _scenario.ShouldHaveAppendedEvent<global::Shop.Shop.Customers.IssueInvite.IssueInvite, global::Shop.Shop.Customers.IssueInvite.InviteIssued>(new global::Shop.Common.CustomerId("cus_01m4t7txm0hrn5rwemy2dkwtgq"), @event => @event.InviteCode == new global::Shop.Common.InviteCode("K7MPQ2XRT9"));
    [global::Xunit.FactAttribute] void should_return_the_expected_response() => ((global::Cratis.Arc.Commands.CommandResult<global::Shop.Common.InviteCode>)_result).Response.ShouldEqual(new global::Shop.Common.InviteCode("K7MPQ2XRT9"));

    public void Dispose() => _scenario.Dispose();
}

#endif
