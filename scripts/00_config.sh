#!/usr/bin/env bash
# Configuracao compartilhada pelos scripts da Sprint 3 de DevOps.
# Pode-se sobrescrever qualquer valor antes da execucao, por exemplo:
#   RM=rm563304 LOCATION=eastus ./scripts/01_create-resource-group.sh

RM="${RM:-rm563304}"
LOCATION="${LOCATION:-eastus}"

RM_NUMBER="${RM#rm}"
RESOURCE_GROUP="${RESOURCE_GROUP:-rg-clyvovet-${RM_NUMBER}}"
APP_SERVICE_PLAN="${APP_SERVICE_PLAN:-plan-clyvovet-${RM_NUMBER}}"
WEBAPP_NAME="${WEBAPP_NAME:-clyvovet-api-${RM_NUMBER}}"
SQL_SERVER_NAME="${SQL_SERVER_NAME:-clyvovet-sql-${RM_NUMBER}}"
SQL_DATABASE_NAME="${SQL_DATABASE_NAME:-ClyvoVetDb}"
SQL_ADMIN_USER="${SQL_ADMIN_USER:-clyvoadmin}"
APP_SERVICE_SKU="${APP_SERVICE_SKU:-F1}"
SQL_DATABASE_SKU="${SQL_DATABASE_SKU:-Basic}"

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"

require_command() {
  if ! command -v "$1" >/dev/null 2>&1; then
    echo "ERRO: o comando '$1' nao foi encontrado." >&2
    exit 1
  fi
}

require_azure_login() {
  require_command az
  if ! az account show >/dev/null 2>&1; then
    echo "ERRO: faca login antes de continuar: az login" >&2
    exit 1
  fi
}

read_sql_password() {
  if [[ -z "${SQL_ADMIN_PASSWORD:-}" ]]; then
    read -r -s -p "Senha do administrador do Azure SQL: " SQL_ADMIN_PASSWORD
    echo
    export SQL_ADMIN_PASSWORD
  fi

  if [[ ${#SQL_ADMIN_PASSWORD} -lt 12 ]]; then
    echo "ERRO: use uma senha forte com pelo menos 12 caracteres." >&2
    exit 1
  fi
}

print_configuration() {
  cat <<EOF
RM                 : $RM
Regiao             : $LOCATION
Resource Group     : $RESOURCE_GROUP
App Service Plan   : $APP_SERVICE_PLAN ($APP_SERVICE_SKU)
Web App            : $WEBAPP_NAME
Azure SQL Server   : $SQL_SERVER_NAME
Azure SQL Database : $SQL_DATABASE_NAME ($SQL_DATABASE_SKU)
EOF
}

