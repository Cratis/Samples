// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MultiTenancy.Specs.for_namespace_isolation;

public class when_using_the_same_item_id : Specification
{
    readonly ItemId _itemId = ItemId.New();
    readonly EventScenario _tenantA = new(EventSequenceId.Log, "MultiTenancy", "tenant-a", null);
    readonly EventScenario _tenantB = new(EventSequenceId.Log, "MultiTenancy", "tenant-b", null);
    readonly EventScenario _default = new(EventSequenceId.Log, "MultiTenancy", EventStoreNamespaceName.Default, null);
    ItemAdded _tenantAItem = null!;
    ItemAdded _tenantBItem = null!;
    ItemAdded _defaultItem = null!;
    EventStoreNamespaceName _tenantANamespace = EventStoreNamespaceName.NotSet;
    EventStoreNamespaceName _tenantBNamespace = EventStoreNamespaceName.NotSet;
    EventStoreNamespaceName _defaultNamespace = EventStoreNamespaceName.NotSet;

    async Task Because()
    {
        var tenantAResult = await _tenantA.EventLog.Append(_itemId, new ItemAdded("Visible only in tenant A"));
        var tenantBResult = await _tenantB.EventLog.Append(_itemId, new ItemAdded("Visible only in tenant B"));
        var defaultResult = await _default.EventLog.Append(_itemId, new ItemAdded("Visible only in Default"));

        _tenantANamespace = tenantAResult.EventStoreNamespace;
        _tenantBNamespace = tenantBResult.EventStoreNamespace;
        _defaultNamespace = defaultResult.EventStoreNamespace;
        _tenantAItem = await ReadItem(_tenantA);
        _tenantBItem = await ReadItem(_tenantB);
        _defaultItem = await ReadItem(_default);
    }

    void Destroy()
    {
        _tenantA.Dispose();
        _tenantB.Dispose();
        _default.Dispose();
    }

    [Fact] void should_append_tenant_a_to_its_namespace() => _tenantANamespace.ShouldEqual((EventStoreNamespaceName)"tenant-a");
    [Fact] void should_append_tenant_b_to_its_namespace() => _tenantBNamespace.ShouldEqual((EventStoreNamespaceName)"tenant-b");
    [Fact] void should_append_default_to_its_namespace() => _defaultNamespace.ShouldEqual(EventStoreNamespaceName.Default);
    [Fact] void should_keep_tenant_a_value() => _tenantAItem.Text.ShouldEqual((ItemText)"Visible only in tenant A");
    [Fact] void should_keep_tenant_b_value() => _tenantBItem.Text.ShouldEqual((ItemText)"Visible only in tenant B");
    [Fact] void should_keep_default_value() => _defaultItem.Text.ShouldEqual((ItemText)"Visible only in Default");

    async Task<ItemAdded> ReadItem(EventScenario scenario)
    {
        var events = await scenario.EventLog.GetFromSequenceNumber(EventSequenceNumber.First, _itemId);
        return events.Select(_ => _.Content).OfType<ItemAdded>().Single();
    }
}
