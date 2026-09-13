#!/usr/bin/env bash
# Passo 4 - executa o DDL e a carga inicial no Azure SQL usando sqlcmd.
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
# shellcheck source=00_config.sh
source "$SCRIPT_DIR/00_config.sh"

require_azure_login
require_command sqlcmd
read_sql_password

echo "== CLYVO VET | Passo 4: Schema e dados iniciais =="

export SQLCMDPASSWORD="$SQL_ADMIN_PASSWORD"
sqlcmd \
  -S "tcp:${SQL_SERVER_NAME}.database.windows.net,1433" \
  -d "$SQL_DATABASE_NAME" \
  -U "$SQL_ADMIN_USER" \
  -N -C -b \
  -i "$SCRIPT_DIR/script_bd.sql"
unset SQLCMDPASSWORD

echo "OK: tabelas, relacionamentos e dados iniciais criados."
echo "Proximo passo: ./scripts/05_deploy-app-service.sh"

