#!/usr/bin/env bash
# Passo 5 - testa, publica e envia a API para o App Service via ZIP deploy.
# O ZIP contem binarios .NET publicados, e nao uma imagem de container.
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
# shellcheck source=00_config.sh
source "$SCRIPT_DIR/00_config.sh"

require_azure_login
require_command dotnet
require_command zip

echo "== CLYVO VET | Passo 5: testes + deploy =="

cd "$PROJECT_DIR"
dotnet restore ChallengeAPI.sln
dotnet test ChallengeAPI.sln --configuration Release --no-restore

rm -rf publish deploy.zip
dotnet publish ChallengeAPI.csproj \
  --configuration Release \
  --output publish \
  --no-restore \
  /p:UseAppHost=false

(
  cd publish
  zip -qr ../deploy.zip .
)

az webapp deploy \
  --resource-group "$RESOURCE_GROUP" \
  --name "$WEBAPP_NAME" \
  --src-path deploy.zip \
  --type zip \
  --clean true \
  --restart true \
  --track-status true

echo "Aguardando a inicializacao da API..."
for attempt in {1..18}; do
  if curl -fsS "https://${WEBAPP_NAME}.azurewebsites.net/health"; then
    echo
    echo "OK: deploy concluido e banco acessivel."
    echo "Swagger: https://${WEBAPP_NAME}.azurewebsites.net/swagger"
    echo "Proximo passo: ./scripts/06_test-crud.sh"
    exit 0
  fi
  echo "Tentativa $attempt/18: API ainda inicializando..."
  sleep 10
done

echo "ERRO: a API nao ficou saudavel no tempo esperado." >&2
echo "Consulte: az webapp log tail -g $RESOURCE_GROUP -n $WEBAPP_NAME" >&2
exit 1

