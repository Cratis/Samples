#!/usr/bin/env bash
# Run the Library Composition with Microsoft SQL Server.
# Usage: ./run-mssql.sh [--docker]
#   --docker  Use docker compose instead of Aspire

set -euo pipefail
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

if [[ "${1:-}" == "--docker" ]]; then
    echo "▶ Starting infrastructure with docker compose (MSSQL profile)..."
    docker compose --profile mssql -f "$SCRIPT_DIR/docker-compose.yml" up -d
    echo "✓ Chronicle    → http://localhost:8080  (Workbench)"
    echo "✓ SQL Server   → localhost:1433"
    echo "✓ Keycloak     → http://localhost:8090  (lending) / :8091 (members)"
else
    echo "▶ Starting Composition AppHost with DATABASE_TYPE=mssql..."
    DATABASE_TYPE=mssql dotnet run --project "$SCRIPT_DIR/Composition/Composition.csproj"
fi
