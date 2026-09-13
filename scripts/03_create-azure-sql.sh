#!/usr/bin/env bash
# Passo 3 - cria o Azure SQL Server e o Azure SQL Database (PaaS),
# libera acesso aos recursos Azure e configura a connection string no Web App.
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
# shellcheck source=00_config.sh
source "$SCRIPT_DIR/00_config.sh"

require_azure_login
read_sql_password

echo "== CLYVO VET | Passo 3: Azure SQL Database =="

if az sql server show --resource-group "$RESOURCE_GROUP" --name "$SQL_SERVER_NAME" >/dev/null 2>&1; then
  echo "SQL Server '$SQL_SERVER_NAME' ja existe."
else
  az sql server create \
    --resource-group "$RESOURCE_GROUP" \
    --name "$SQL_SERVER_NAME" \
    --location "$LOCATION" \
    --admin-user "$SQL_ADMIN_USER" \
    --admin-password "$SQL_ADMIN_PASSWORD" \
    --output table
fi

if az sql db show --resource-group "$RESOURCE_GROUP" --server "$SQL_SERVER_NAME" --name "$SQL_DATABASE_NAME" >/dev/null 2>&1; then
  echo "Banco '$SQL_DATABASE_NAME' ja existe."
else
  az sql db create \
    --resource-group "$RESOURCE_GROUP" \
    --server "$SQL_SERVER_NAME" \
    --name "$SQL_DATABASE_NAME" \
    --service-objective "$SQL_DATABASE_SKU" \
    --backup-storage-redundancy Local \
    --output table
fi

# 0.0.0.0 representa a regra especial que permite conexoes de servicos Azure,
# incluindo o App Service, sem expor a senha no codigo-fonte.
az sql server firewall-rule create \
  --resource-group "$RESOURCE_GROUP" \
  --server "$SQL_SERVER_NAME" \
  --name AllowAzureServices \
  --start-ip-address 0.0.0.0 \
  --end-ip-address 0.0.0.0 \
  --output none

# Libera o IP usado na demonstracao para que sqlcmd consiga executar o DDL.
CURRENT_IP="$(curl -fsS https://api.ipify.org || true)"
if [[ -n "$CURRENT_IP" ]]; then
  az sql server firewall-rule create \
    --resource-group "$RESOURCE_GROUP" \
    --server "$SQL_SERVER_NAME" \
    --name DevOpsDemoClient \
    --start-ip-address "$CURRENT_IP" \
    --end-ip-address "$CURRENT_IP" \
    --output none
  echo "IP da demonstracao liberado: $CURRENT_IP"
else
  echo "AVISO: nao foi possivel detectar o IP publico."
fi

CONNECTION_STRING="Server=tcp:${SQL_SERVER_NAME}.database.windows.net,1433;Initial Catalog=${SQL_DATABASE_NAME};Persist Security Info=False;User ID=${SQL_ADMIN_USER};Password=${SQL_ADMIN_PASSWORD};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

az webapp config connection-string set \
  --resource-group "$RESOURCE_GROUP" \
  --name "$WEBAPP_NAME" \
  --connection-string-type SQLAzure \
  --settings SqlServerConnection="$CONNECTION_STRING" \
  --output none

echo "OK: banco PaaS e connection string do App Service configurados."
echo "Proximo passo: ./scripts/04_initialize-database.sh"

