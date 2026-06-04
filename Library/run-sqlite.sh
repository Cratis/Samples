#!/usr/bin/env bash
# Run the Library Composition with SQLite.
# Usage: ./run-sqlite.sh [--docker]
#   --docker  Use docker compose instead of Aspire

set -euo pipefail
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

if [[ "${1:-}" == "--docker" ]]; then
    echo "▶ Starting infrastructure with docker compose (SQLite profile)..."
    docker compose --profile sqlite -f "$SCRIPT_DIR/Composition/docker-compose.yml" up -d
    echo "✓ Chronicle  → http://localhost:8080  (Workbench)"
    echo "✓ Keycloak   → http://localhost:8090  (lending) / :8091 (members)"
else
    echo "▶ Starting Composition AppHost with DATABASE_TYPE=sqlite..."
    DATABASE_TYPE=sqlite dotnet run --project "$SCRIPT_DIR/Composition/Composition.csproj"
fi
