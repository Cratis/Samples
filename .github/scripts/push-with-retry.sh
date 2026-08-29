#!/usr/bin/env bash
#
# Push the current branch, recovering from a push that lost a race.
#
# The dependency update jobs commit straight to main from several jobs at once,
# so a rejected push is an expected outcome rather than a failure: another job
# simply moved the branch first. Rebase the local commits onto the new remote
# head and try again.

set -euo pipefail

attempts=${PUSH_RETRY_ATTEMPTS:-5}

for ((attempt = 1; attempt <= attempts; attempt++)); do
    if git push; then
        echo "Pushed on attempt ${attempt} of ${attempts}"
        exit 0
    fi

    if [ "${attempt}" -eq "${attempts}" ]; then
        break
    fi

    echo "Push attempt ${attempt} of ${attempts} was rejected - another job moved the branch. Rebasing onto the latest remote head and retrying..."
    sleep "$((attempt * 5))"

    if ! git pull --rebase; then
        git rebase --abort || true
        echo "::error::Could not rebase onto the latest remote head"
        exit 1
    fi
done

echo "::error::Failed to push after ${attempts} attempts"
exit 1
