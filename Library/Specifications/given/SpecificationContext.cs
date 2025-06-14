// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Library.given;

#pragma warning disable CS0436

public class SpecificationContext(ChronicleOutOfProcessFixture fixture) : IntegrationSpecificationContext<ChronicleOutOfProcessFixture, ApiWebApplicationFactory, Program>(fixture);
