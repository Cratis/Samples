// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

global using System.Reactive.Subjects;
global using Cratis.Arc.Commands;
global using Cratis.Arc.Commands.ModelBound;
global using Cratis.Arc.Queries;
global using Cratis.Arc.Queries.ModelBound;
global using Cratis.Chronicle.Events;
global using Cratis.Chronicle.Events.Constraints;
global using Cratis.Chronicle.EventSequences;
global using Cratis.Chronicle.Observation;
global using Cratis.Chronicle.Projections;
global using Cratis.Chronicle.Reactors;
global using Cratis.Chronicle.ReadModels;
global using Cratis.Chronicle.Reducers;
global using Cratis.Chronicle.Transactions;
global using Cratis.Concepts;
global using Microsoft.AspNetCore.Mvc;
#if DEBUG
global using Cratis.Chronicle.XUnit;
global using Cratis.Chronicle.XUnit.Integration;
global using Cratis.Specifications;
global using NSubstitute;
global using NSubstitute.Core;
global using Xunit;
#endif
