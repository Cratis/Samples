// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MultiTenancy.Specs.for_Item;

public class when_an_item_is_added : Specification
{
    readonly ItemId _itemId = ItemId.New();
    readonly ItemText _text = "Project into this tenant";
    readonly ReadModelScenario<Item> _scenario = new();

    async Task Because() =>
        await _scenario.Given.ForEventSource(_itemId).Events(new ItemAdded(_text));

    [Fact] void should_use_the_event_source_id() => _scenario.Instance.Id.ShouldEqual(_itemId);
    [Fact] void should_project_the_item_text() => _scenario.Instance.Text.ShouldEqual(_text);
}
