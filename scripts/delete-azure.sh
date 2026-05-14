#!/bin/bash
RESOURCE_GROUP="rg-challengeapi"

echo "DELETANDO todos os recursos Azure..."
read -p "Tem certeza? (s/N): " CONFIRMA
if [[ "$CONFIRMA" != "s" && "$CONFIRMA" != "S" ]]; then
  echo "Cancelado."
  exit 0
fi

az group delete --name "$RESOURCE_GROUP" --yes --no-wait
echo "Remocao iniciada! Tire um PRINT do portal Azure como evidencia."  