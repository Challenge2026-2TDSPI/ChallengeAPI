#!/usr/bin/env bash
# Responsabilidade: criar o Web App e definir runtime, inicializacao e HTTPS.
set -euo pipefail
source "$(dirname "${BASH_SOURCE[0]}")/config.sh"

if [[ ! "$WEBAPP_NAME" =~ ^[a-z0-9][a-z0-9-]{0,58}[a-z0-9]$ ]]; then
  echo "Preencha WEBAPP_NAME em scripts/config.sh (minusculas, numeros e hifens)." >&2
  exit 1
fi

PLAN_ID="$(az appservice plan show --name "$PLAN_NAME" \
  --resource-group "$RESOURCE_GROUP" --query id --output tsv)"
CURRENT_PLAN="$(az webapp list --resource-group "$RESOURCE_GROUP" \
  --query "[?name=='$WEBAPP_NAME'].serverFarmId | [0]" --output tsv)"

if [[ -z "$CURRENT_PLAN" ]]; then
  az webapp create \
    --name "$WEBAPP_NAME" --resource-group "$RESOURCE_GROUP" \
    --plan "$PLAN_ID" --runtime 'DOTNETCORE:8.0' \
    --only-show-errors --output none
elif [[ "${CURRENT_PLAN,,}" != "${PLAN_ID,,}" ]]; then
  echo "Esse Web App ja esta ligado a outro plano. Confira config.sh." >&2
  exit 1
fi

az webapp config set \
  --name "$WEBAPP_NAME" --resource-group "$RESOURCE_GROUP" \
  --linux-fx-version 'DOTNETCORE|8.0' \
  --startup-file 'dotnet ChallengeAPI.dll' --min-tls-version 1.2 \
  --only-show-errors --output none
az webapp update \
  --name "$WEBAPP_NAME" --resource-group "$RESOURCE_GROUP" \
  --https-only true --only-show-errors --output none

echo "Web App pronto: $WEBAPP_NAME"
if [[ -n "${TF_BUILD:-}" ]]; then
  echo "##vso[task.setvariable variable=webAppName;isOutput=true]$WEBAPP_NAME"
fi

