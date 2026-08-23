// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace OperationsDiagnosis.Specs.for_FailingProbeReactor;

public class when_receiving_the_failure_fixture : Specification
{
    Exception? _exception;

    void Because() => _exception = Catch.Exception(
        () => new FailingProbeReactor().Requested(new ProbeRequested(ProbeName.FailureFixture)));

    [Fact] void should_fail_with_the_diagnostic_exception() => _exception.ShouldBeOfExactType<ProbeConfiguredToFail>();
    [Fact] void should_include_the_stable_diagnostic_marker() => _exception.Message.ShouldEqual("OD-001: Probe 'operations-diagnosis-canary' is configured to fail for diagnosis.");
}
