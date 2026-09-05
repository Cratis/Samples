// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle;

namespace Chronicle.Backend;

static class ChronicleConfiguration
{
    public const string EventStore = "ChronicleBackend";
    public static readonly EventStoreNamespaceName Namespace = EventStoreNamespaceName.Default;
}
