#!/usr/bin/env bash
# Responsabilidade: criar ou reutilizar o plano Linux, sem mudar o SKU existente.
set -euo pipefail
source "$(dirname "${BASH_SOURCE[0]}")/config.sh"

if [[ "$SKU" != "F1" ]]; then
  echo "Somente F1 (Free) e permitido neste projeto. Nenhum plano pago sera criado." >&2
  exit 1
fi

PLAN_ID="$(az appservice plan list --resource-group "$RESOURCE_GROUP" \
  --query "[?name=='$PLAN_NAME'].id | [0]" --output tsv)"

if [[ -n "$PLAN_ID" ]]; then
  CURRENT_SKU="$(az appservice plan show --ids "$PLAN_ID" --query sku.name --output tsv)"
  IS_LINUX="$(az appservice plan show --ids "$PLAN_ID" --query reserved --output tsv)"
  if [[ "$CURRENT_SKU" != "$SKU" || "$IS_LINUX" != "true" ]]; then
    echo "O plano existente tem outro SKU ou sistema operacional. Confira config.sh." >&2
    exit 1
  fi
  echo "Plano reutilizado: $PLAN_NAME"
  exit 0
fi

az appservice plan create \
  --name "$PLAN_NAME" --resource-group "$RESOURCE_GROUP" \
  --location "$LOCATION" --is-linux --sku "$SKU" \
  --only-show-errors --output none

echo "Plano criado: $PLAN_NAME ($SKU)"

