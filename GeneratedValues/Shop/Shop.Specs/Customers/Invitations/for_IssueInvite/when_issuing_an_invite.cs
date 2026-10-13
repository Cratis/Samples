// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Shop.Customers.Invitations;

namespace Shop.Specs.Customers.Invitations.for_IssueInvite;

public class when_issuing_an_invite : Specification
{
    readonly CommandScenario<IssueInvite> _scenario = new();
    readonly CustomerId _customerId = new("cus_01m4t7txm0hrn5rwemy2dkwtgq");
    readonly InviteCode _inviteCode = new("K7MPQ2XRT9");
    CommandResult _result = null!;

    void Establish() => _scenario.PinGeneratedValue("inviteCode", _inviteCode);
    async Task Because() => _result = await _scenario.Execute(new IssueInvite(_customerId));
    void Destroy() => _scenario.Dispose();

    [Fact] void should_succeed() => _result.ShouldBeSuccessful();
    [Fact] void should_return_the_pinned_invite_code() => ((CommandResult<InviteCode>)_result).Response.ShouldEqual(_inviteCode);
    [Fact] Task should_record_the_invite_on_the_customer() => _scenario.ShouldHaveAppendedEvent<IssueInvite, InviteIssued>(_customerId, @event => @event.InviteCode == _inviteCode);
}
