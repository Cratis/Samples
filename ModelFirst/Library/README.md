# Model-first Library

This bounded sample exercises the current experimental model-first tooling: author one Cratis Screenplay file, compile it, and ask Stage to run the specifications embedded in the model.

`library.play` contains one catalog state change:

- strongly typed book, title, and author concepts;
- an `AddBook` command with two required-value rules;
- the `BookAddedToCatalog` fact; and
- one accepted-path and one rejected-path model specification.

## Verify it

The sample intentionally has no project or dependency manifest. It uses the dependencies and command-line entry points owned by current local Screenplay and Stage checkouts.

From this directory, point the variables at already-built sibling checkouts and run. `--no-build --no-restore` keeps this verification offline and uses those product-owned build outputs:

```shell
export SCREENPLAY_REPO=/path/to/Screenplay
export STAGE_REPO=/path/to/Stage

# Compile the model and fail on warnings.
dotnet run --no-build --no-restore \
  --project "$SCREENPLAY_REPO/Source/DotNET/Tool/Tool.csproj" -- \
  "$PWD/library.play" --warnaserror

# Run Stage's model-level specifications.
RESULTS_PATH="${TMPDIR:-/tmp}/cratis-library-stage-results.json"
dotnet run --no-build --no-restore \
  --project "$STAGE_REPO/Source/SpecRunner/SpecRunner.csproj" -- \
  --model "$PWD" --output "$RESULTS_PATH"

cat "$RESULTS_PATH"
```

A successful run compiles one file with no diagnostics and records two `Passed` specification outcomes. Read the result file rather than relying only on the Stage process exit code: the runner uses exit code `0` when a run completes even if an individual model specification fails.

## Truthful scope

Stage's specification runner verifies this sample at the **model level**: referenced commands and events resolve, and the required-value rules agree with the expected rejection. It does not execute the command against a live Chronicle.

This bounded sample does **not** demonstrate query results, Scene UI, complete source generation, or a Studio round trip. Those are intentionally left for later samples.
