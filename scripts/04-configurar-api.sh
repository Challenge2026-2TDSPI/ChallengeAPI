#!/usr/bin/env bash
# Responsabilidade: salvar as configuracoes da API no Web App.
# No Cloud Shell, pergunta os segredos sem exibir a digitacao.
# Na pipeline, recebe ORACLE_CONNECTION, JWT_* e MONGO_* pelo ambiente.
# Python e usado somente para leitura oculta e escrita segura de JSON.
set -euo pipefail
set +x
umask 077
source "$(dirname "${BASH_SOURCE[0]}")/config.sh"

WORK_DIR="$(mktemp -d)"
chmod 700 "$WORK_DIR"
trap 'rm -rf "$WORK_DIR"' EXIT
export ASK_SECRETS=false
[[ -t 0 ]] && export ASK_SECRETS=true

python3 - "$WORK_DIR/settings.json" <<'PY'
import getpass
import json
import os
import sys
import warnings

interactive = os.environ["ASK_SECRETS"] == "true"
warnings.simplefilter("error", getpass.GetPassWarning)

def env_value(name):
    value = os.environ.get(name, "")
    return "" if value.startswith("$(") else value

def read_value(name, prompt, default="", required=False):
    value = env_value(name)
    if not value and interactive:
        try:
            value = getpass.getpass(prompt)
        except getpass.GetPassWarning:
            raise SystemExit("Terminal sem leitura oculta. Use as variaveis secretas do Azure DevOps.")
    value = value or default
    if required and not value:
        raise SystemExit("Configure " + name + ".")
    return value

oracle = read_value("ORACLE_CONNECTION", "String Oracle completa: ", required=True)
jwt = read_value("JWT_SECRET_KEY", "Chave JWT atual da API: ", required=True)
if len(jwt.encode("utf-8")) < 32:
    raise SystemExit("JWT_SECRET_KEY deve ter pelo menos 32 bytes.")

settings = {
    "ConnectionStrings__OracleConnection": oracle,
    "Jwt__SecretKey": jwt,
    "Jwt__Issuer": env_value("JWT_ISSUER") or "ChallengeAPI",
    "Jwt__Audience": env_value("JWT_AUDIENCE") or "ChallengeAPI",
    "ASPNETCORE_ENVIRONMENT": "Production",
    "SCM_DO_BUILD_DURING_DEPLOYMENT": "false",
}
mongo = read_value("MONGO_CONNECTION", "Conexao MongoDB (Enter para manter): ")
if mongo:
    settings["MongoDbSettings__ConnectionString"] = mongo
    settings["MongoDbSettings__DatabaseName"] = read_value(
        "MONGO_DATABASE", "Nome do banco MongoDB: ", required=True)

with open(sys.argv[1], "x", encoding="utf-8") as stream:
    json.dump(settings, stream, ensure_ascii=False)
PY

# @arquivo evita expor valores em argumentos; nao imprimir a resposta da Azure.
if ! az webapp config appsettings set \
  --name "$WEBAPP_NAME" --resource-group "$RESOURCE_GROUP" \
  --settings "@$WORK_DIR/settings.json" \
  --only-show-errors --output none 2>"$WORK_DIR/erro.log"; then
  echo "Falha ao salvar configuracoes. Confira permissoes e dados no portal." >&2
  exit 1
fi

echo "Configuracoes salvas. Quando informado, o MongoDB tambem foi atualizado."

