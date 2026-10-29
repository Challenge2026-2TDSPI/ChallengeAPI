#!/usr/bin/env bash
# Responsabilidade: criar ou reutilizar o grupo de recursos.
set -euo pipefail
source "$(dirname "${BASH_SOURCE[0]}")/config.sh"

az account show --query '{Nome:name,ID:id}' --output table
GROUP_EXISTS="$(az group exists --name "$RESOURCE_GROUP" --output tsv)"
if [[ "$GROUP_EXISTS" != "true" ]]; then
  az group create \
    --name "$RESOURCE_GROUP" \
    --location "$LOCATION" \
    --only-show-errors --output none
fi

echo "Grupo pronto: $RESOURCE_GROUP"

