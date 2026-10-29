#!/usr/bin/env bash
# Edite somente os dados nao secretos aqui.
# Selecione a assinatura no Cloud Shell com: az account set --subscription ID
# Execute os scripts 01, 02, 03 e 04 nessa ordem. O 05 e usado apos o deploy.
# A pipeline usa este mesmo arquivo. Nao coloque senhas nele.

export RESOURCE_GROUP="${RESOURCE_GROUP:-rg-challengeapi-sprint4}"
export PLAN_NAME="${PLAN_NAME:-plan-challengeapi-sprint4}"
export WEBAPP_NAME="${WEBAPP_NAME:-clyvovet-api-rm562156}"
export LOCATION="${LOCATION:-brazilsouth}"
export SKU="${SKU:-F1}"
# Se houver bloqueio de politica, troque brazilsouth por mexicocentral aqui.
# A regiao alternativa tambem depende de permissao e disponibilidade de F1.
# Somente F1 (Free). Se indisponivel, a criacao falha; nao usa plano pago.

