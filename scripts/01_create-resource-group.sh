#!/usr/bin/env bash
# Passo 1 - cria o Resource Group e registra os provedores necessarios.
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
# shellcheck source=00_config.sh
source "$SCRIPT_DIR/00_config.sh"

require_azure_login

echo "== CLYVO VET | Passo 1: Resource Group =="
print_configuration

az provider register --namespace Microsoft.Web --wait
az provider register --namespace Microsoft.Sql --wait

if az group show --name "$RESOURCE_GROUP" >/dev/null 2>&1; then
  echo "Resource Group '$RESOURCE_GROUP' ja existe."
else
  az group create \
    --name "$RESOURCE_GROUP" \
    --location "$LOCATION" \
    --output table
fi

echo "OK: Resource Group pronto."
echo "Proximo passo: ./scripts/02_create-app-service.sh"

