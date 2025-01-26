// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Quickstart;

public record MainMenuItem(MainMenuCommand Command, string Text)
{
    public override string ToString() => Text;
}
