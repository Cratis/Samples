#!/usr/bin/env bash
# Run the Library Composition with PostgreSQL.
# Usage: ./run-postgresql.sh [--docker]
#   --docker  Use docker compose instead of Aspire

set -euo pipefail
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

if [[ "${1:-}" == "--docker" ]]; then
    echo "▶ Starting infrastructure with docker compose (PostgreSQL profile)..."
    docker compose --profile postgresql -f "$SCRIPT_DIR/Composition/docker-compose.yml" up -d
    echo "✓ Chronicle  → http://localhost:8080  (Workbench)"
    echo "✓ PostgreSQL → localhost:5432"
    echo "✓ Keycloak   → http://localhost:8090  (lending) / :8091 (members)"
else
    echo "▶ Starting Composition AppHost with DATABASE_TYPE=postgresql..."
    DATABASE_TYPE=postgresql dotnet run --project "$SCRIPT_DIR/Composition/Composition.csproj"
fi
