#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
URL="${VOYAGO_E2E_BASE_URL:-http://localhost:65138}"
EMAIL="${VOYAGO_E2E_EMAIL:-voyago-e2e@local.test}"
PASSWORD="${VOYAGO_E2E_PASSWORD:-Voyago.E2E.2026!}"
ID="$(python -c 'import uuid; print(uuid.uuid4().hex)')"
DIR="$ROOT/.e2e-data"
DB_BASH="$DIR/voyago-e2e-$ID.db"
LOG="$DIR/voyago-e2e-server-$ID.log"
mkdir -p "$DIR"

if command -v cygpath >/dev/null 2>&1; then
  DB_DOTNET="$(cygpath -w "$DB_BASH")"
else
  DB_DOTNET="$DB_BASH"
fi

export ASPNETCORE_ENVIRONMENT=Testing
export ASPNETCORE_URLS="$URL"
export ConnectionStrings__DefaultConnection="Data Source=$DB_DOTNET;Foreign Keys=True"
export E2ETestUser__Email="$EMAIL"
export E2ETestUser__Password="$PASSWORD"
export VOYAGO_E2E_BASE_URL="$URL"
export VOYAGO_E2E_EMAIL="$EMAIL"
export VOYAGO_E2E_PASSWORD="$PASSWORD"

PID=""
cleanup() {
  code=$?
  trap - EXIT INT TERM
  echo
  echo "Finalizando entorno E2E..."
  if [[ -n "$PID" ]]; then
    kill "$PID" 2>/dev/null || true
    wait "$PID" 2>/dev/null || true
  fi
  rm -f "$DB_BASH" "${DB_BASH}-shm" "${DB_BASH}-wal"
  if [[ $code -eq 0 ]]; then
    rm -f "$LOG"
    echo "Regresion E2E completada correctamente."
  else
    echo "La regresion E2E termino con errores."
    echo "Log: $LOG"
  fi
  exit "$code"
}
trap cleanup EXIT INT TERM

cd "$ROOT"
for file in Voyago.sln Voyago/Voyago.csproj tests/Voyago.E2E/Voyago.E2E.csproj; do
  [[ -f "$file" ]] || { echo "ERROR: no se encontro $file"; exit 1; }
done
command -v curl >/dev/null 2>&1 || { echo "ERROR: curl no esta disponible"; exit 1; }

echo "Restaurando y compilando..."
dotnet restore Voyago.sln
dotnet build Voyago.sln --no-restore

echo "Iniciando Voyago E2E en $URL..."
dotnet run --project Voyago/Voyago.csproj --no-build --no-launch-profile >"$LOG" 2>&1 &
PID=$!

READY=false
for attempt in $(seq 1 60); do
  kill -0 "$PID" 2>/dev/null || { cat "$LOG"; exit 1; }
  if curl --silent --fail "$URL/api/v1/health" >/dev/null 2>&1; then
    READY=true
    echo "Servidor disponible despues de $attempt segundo(s)."
    break
  fi
  sleep 1
done

if [[ "$READY" != true ]]; then
  echo "ERROR: el servidor no respondio a tiempo."
  cat "$LOG"
  exit 1
fi

echo "Ejecutando Playwright..."
dotnet test tests/Voyago.E2E/Voyago.E2E.csproj --no-build --logger "console;verbosity=detailed"
