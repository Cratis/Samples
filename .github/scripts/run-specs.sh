#!/usr/bin/env bash
# Copyright (c) Cratis. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# Accept dotnet run arguments, including -- before native xUnit runner options.
set -euo pipefail

report_directory=$(mktemp -d)
trap 'rm -rf -- "$report_directory"' EXIT
report="$report_directory/results.xml"

# set -e preserves a native failure without inspecting or replacing its exit code.
dotnet run "$@" -result-xml "$report"

python3 - "$report" <<'PY'
import re
import sys
import xml.etree.ElementTree as ET

try:
    root = ET.parse(sys.argv[1]).getroot()
    if root.tag != "assemblies":
        raise ValueError("expected an assemblies root")
    assemblies = list(root)
    if not assemblies or any(assembly.tag != "assembly" for assembly in assemblies):
        raise ValueError("expected assembly results")
    totals = [assembly.get("total", "") for assembly in assemblies]
    if any(re.fullmatch(r"[0-9]+", total) is None for total in totals):
        raise ValueError("expected nonnegative integer assembly totals")
    if sum(int(total) for total in totals) == 0:
        raise ValueError("no tests selected")
except (OSError, ET.ParseError, ValueError) as error:
    print(f"Invalid xUnit report {sys.argv[1]}: {error}", file=sys.stderr)
    sys.exit(1)
PY
