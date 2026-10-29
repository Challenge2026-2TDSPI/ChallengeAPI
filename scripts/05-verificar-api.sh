#!/usr/bin/env bash
# Responsabilidade: verificar se a API publicada e o Oracle estao respondendo.
set -euo pipefail
source "$(dirname "${BASH_SOURCE[0]}")/config.sh"

APP_HOST="$(az webapp show --name "$WEBAPP_NAME" \
  --resource-group "$RESOURCE_GROUP" --query defaultHostName --output tsv)"

echo "Verificando OpenAPI..."
curl --fail --location --silent --show-error \
  --retry 10 --retry-all-errors --retry-delay 10 \
  --connect-timeout 10 --max-time 30 \
  "https://$APP_HOST/swagger/v1/swagger.json" --output /dev/null

echo "Verificando Oracle..."
curl --fail --location --silent --show-error \
  --retry 3 --retry-all-errors --retry-delay 5 \
  --connect-timeout 10 --max-time 30 \
  "https://$APP_HOST/health" --output /dev/null

echo
echo "API e Oracle responderam com sucesso."
echo "Swagger: https://$APP_HOST/swagger"

