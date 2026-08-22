// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MultiTenancy.Specs.for_AddItem;

public class when_adding_an_item : Specification
{
    readonly ItemText _text = "Review the tenant boundary";
    ItemAdded _result = null!;

    void Because() => _result = new AddItem(ItemId.New(), _text).Handle();

    [Fact] void should_record_the_item_text() => _result.Text.ShouldEqual(_text);
}
