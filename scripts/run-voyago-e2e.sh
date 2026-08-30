#!/usr/bin/env bash

set -euo pipefail

PROJECT_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"

if command -v python >/dev/null 2>&1; then
    DATABASE_ID="$(python -c 'import uuid; print(uuid.uuid4().hex)')"
elif command -v py >/dev/null 2>&1; then
    DATABASE_ID="$(py -3 -c 'import uuid; print(uuid.uuid4().hex)')"
else
    echo "Error: no se encontro python ni py."
    exit 1
fi

DATABASE_DIR="$PROJECT_ROOT/.e2e-data"
DATABASE_PATH_BASH="$DATABASE_DIR/voyago-e2e-${DATABASE_ID}.db"

BASE_URL="http://localhost:65138"

mkdir -p "$DATABASE_DIR"

if command -v cygpath >/dev/null 2>&1; then
    DATABASE_PATH_DOTNET="$(cygpath -w "$DATABASE_PATH_BASH")"
else
    DATABASE_PATH_DOTNET="$DATABASE_PATH_BASH"
fi

export ASPNETCORE_ENVIRONMENT="Testing"
export ASPNETCORE_URLS="$BASE_URL"

export ConnectionStrings__DefaultConnection="Data Source=${DATABASE_PATH_DOTNET};Foreign Keys=True"

export E2ETestUser__Email="${VOYAGO_E2E_EMAIL:-voyago-e2e@local.test}"
export E2ETestUser__Password="${VOYAGO_E2E_PASSWORD:-Voyago.E2E.2026!}"

echo "Voyago E2E"
echo "URL: $BASE_URL"
echo "SQLite Bash: $DATABASE_PATH_BASH"
echo "SQLite .NET: $DATABASE_PATH_DOTNET"
echo "Usuario: $E2ETestUser__Email"
echo "Proyecto: $PROJECT_ROOT"

cleanup() {
    echo
    echo "Eliminando SQLite temporal..."

    rm -f "$DATABASE_PATH_BASH"
    rm -f "${DATABASE_PATH_BASH}-shm"
    rm -f "${DATABASE_PATH_BASH}-wal"

    echo "Limpieza finalizada."
}

trap cleanup EXIT INT TERM

cd "$PROJECT_ROOT"

if [[ ! -f "Voyago.sln" ]]; then
    echo "Error: no se encontro Voyago.sln en:"
    echo "$PROJECT_ROOT"
    exit 1
fi

if [[ ! -f "Voyago/Voyago.csproj" ]]; then
    echo "Error: no se encontro Voyago/Voyago.csproj."
    exit 1
fi

dotnet run \
    --project Voyago/Voyago.csproj \
    --no-launch-profile
