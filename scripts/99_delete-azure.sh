#!/usr/bin/env bash
# Remove o Resource Group e todos os recursos da demonstracao.
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
# shellcheck source=00_config.sh
source "$SCRIPT_DIR/00_config.sh"

require_azure_login

echo "Resource Group a remover: $RESOURCE_GROUP"
read -r -p "Tem certeza? Digite REMOVER para confirmar: " CONFIRMATION
if [[ "$CONFIRMATION" != "REMOVER" ]]; then
  echo "Operacao cancelada."
  exit 0
fi

az group delete --name "$RESOURCE_GROUP" --yes --no-wait
echo "Remocao iniciada. A operacao ocorre de forma assincrona."
