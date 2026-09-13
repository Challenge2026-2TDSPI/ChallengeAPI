#!/usr/bin/env bash
# Passo 6 - demonstra CRUD em duas tabelas relacionadas (Tutores e Pets).
# Cada alteracao feita pela API e comprovada imediatamente com SELECT no banco.
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
# shellcheck source=00_config.sh
source "$SCRIPT_DIR/00_config.sh"

require_azure_login
require_command curl
require_command jq
require_command sqlcmd
read_sql_password

BASE_URL="https://${WEBAPP_NAME}.azurewebsites.net"
RUN_ID="$(date +%s)"
export SQLCMDPASSWORD="$SQL_ADMIN_PASSWORD"

run_select() {
  sqlcmd \
    -S "tcp:${SQL_SERVER_NAME}.database.windows.net,1433" \
    -d "$SQL_DATABASE_NAME" \
    -U "$SQL_ADMIN_USER" \
    -N -C -b \
    -Q "$1"
}

echo "== 1. INSERT em Tutores pela API + SELECT no Azure SQL =="
TUTOR_RESPONSE="$(curl -fsS -X POST "$BASE_URL/api/Tutores" \
  -H 'Content-Type: application/json' \
  -d "{\"nome\":\"Tutor DevOps ${RUN_ID}\",\"telefone\":\"11999990000\",\"email\":\"devops.${RUN_ID}@clyvovet.com.br\"}")"
echo "$TUTOR_RESPONSE" | jq .
TUTOR_ID="$(echo "$TUTOR_RESPONSE" | jq -r '.id')"
run_select "SELECT * FROM dbo.Tutores WHERE Id = ${TUTOR_ID};"

echo "== 2. INSERT em Pets pela API + SELECT no Azure SQL =="
PET_RESPONSE="$(curl -fsS -X POST "$BASE_URL/api/Pets" \
  -H 'Content-Type: application/json' \
  -d "{\"nome\":\"Nina DevOps\",\"especie\":\"Cao\",\"raca\":\"SRD\",\"idade\":3,\"tutorId\":${TUTOR_ID},\"clinicaId\":1}")"
echo "$PET_RESPONSE" | jq .
PET_ID="$(echo "$PET_RESPONSE" | jq -r '.id')"
run_select "SELECT * FROM dbo.Pets WHERE Id = ${PET_ID};"

echo "== 3. UPDATE em Tutores pela API + SELECT no Azure SQL =="
curl -fsS -X PUT "$BASE_URL/api/Tutores/$TUTOR_ID" \
  -H 'Content-Type: application/json' \
  -d "{\"id\":${TUTOR_ID},\"nome\":\"Tutor Atualizado ${RUN_ID}\",\"telefone\":\"11988880000\",\"email\":\"devops.${RUN_ID}@clyvovet.com.br\"}"
run_select "SELECT * FROM dbo.Tutores WHERE Id = ${TUTOR_ID};"

echo "== 4. UPDATE em Pets pela API + SELECT no Azure SQL =="
curl -fsS -X PUT "$BASE_URL/api/Pets/$PET_ID" \
  -H 'Content-Type: application/json' \
  -d "{\"id\":${PET_ID},\"nome\":\"Nina Atualizada\",\"especie\":\"Cao\",\"raca\":\"SRD\",\"idade\":4,\"tutorId\":${TUTOR_ID},\"clinicaId\":1}"
run_select "SELECT * FROM dbo.Pets WHERE Id = ${PET_ID};"

echo "== 5. SELECT/consulta pela API =="
curl -fsS "$BASE_URL/api/Tutores/$TUTOR_ID" | jq .
curl -fsS "$BASE_URL/api/Pets/$PET_ID" | jq .

echo "== 6. DELETE em Pets pela API + SELECT comprovando a exclusao =="
curl -fsS -X DELETE "$BASE_URL/api/Pets/$PET_ID"
run_select "SELECT * FROM dbo.Pets WHERE Id = ${PET_ID};"

echo "== 7. DELETE em Tutores pela API + SELECT comprovando a exclusao =="
curl -fsS -X DELETE "$BASE_URL/api/Tutores/$TUTOR_ID"
run_select "SELECT * FROM dbo.Tutores WHERE Id = ${TUTOR_ID};"

unset SQLCMDPASSWORD
echo "OK: CRUD completo demonstrado em duas tabelas relacionadas."

