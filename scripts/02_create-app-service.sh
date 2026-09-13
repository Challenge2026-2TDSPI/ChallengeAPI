#!/usr/bin/env bash
# Passo 2 - cria o App Service Plan Linux e o Web App com runtime .NET 8.
# O deploy e code-based: nenhuma imagem ou tecnologia de container e utilizada.
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
# shellcheck source=00_config.sh
source "$SCRIPT_DIR/00_config.sh"

require_azure_login

echo "== CLYVO VET | Passo 2: App Service =="

if az appservice plan show --resource-group "$RESOURCE_GROUP" --name "$APP_SERVICE_PLAN" >/dev/null 2>&1; then
  echo "App Service Plan '$APP_SERVICE_PLAN' ja existe."
else
  az appservice plan create \
    --resource-group "$RESOURCE_GROUP" \
    --name "$APP_SERVICE_PLAN" \
    --location "$LOCATION" \
    --is-linux \
    --sku "$APP_SERVICE_SKU" \
    --output table
fi

if az webapp show --resource-group "$RESOURCE_GROUP" --name "$WEBAPP_NAME" >/dev/null 2>&1; then
  echo "Web App '$WEBAPP_NAME' ja existe."
else
  az webapp create \
    --resource-group "$RESOURCE_GROUP" \
    --plan "$APP_SERVICE_PLAN" \
    --name "$WEBAPP_NAME" \
    --runtime "DOTNETCORE:8.0" \
    --output table
fi

az webapp config set \
  --resource-group "$RESOURCE_GROUP" \
  --name "$WEBAPP_NAME" \
  --linux-fx-version "DOTNETCORE|8.0" \
  --http20-enabled true \
  --always-on false \
  --output none

az webapp config appsettings set \
  --resource-group "$RESOURCE_GROUP" \
  --name "$WEBAPP_NAME" \
  --settings \
    ASPNETCORE_ENVIRONMENT=Production \
    SCM_DO_BUILD_DURING_DEPLOYMENT=false \
  --output none

az webapp log config \
  --resource-group "$RESOURCE_GROUP" \
  --name "$WEBAPP_NAME" \
  --application-logging filesystem \
  --level information \
  --web-server-logging filesystem \
  --output none

echo "OK: https://${WEBAPP_NAME}.azurewebsites.net"
echo "Proximo passo: ./scripts/03_create-azure-sql.sh"

