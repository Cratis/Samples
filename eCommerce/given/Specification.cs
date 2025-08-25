// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace eCommerce.given;

#pragma warning disable CS0436

public class Specification(ChronicleOutOfProcessFixture fixture) : Specification<ChronicleOutOfProcessFixture, ApiWebApplicationFactory, Program>(fixture);
