# Roteiro do video - Sprint 3 DevOps

O video e a prova da entrega. Grave em pelo menos 720p, com voz clara, e deixe
visiveis o terminal, o portal Azure, o Swagger e os resultados do banco.

## 1. Abertura

- Apresente a CLYVO VET e o problema resolvido.
- Explique que foi escolhida a opcao App Service com banco PaaS.
- Mostre `docs/arquitetura-app-service.png` e explique os recursos.
- Mostre rapidamente as quatro tabelas core e seus relacionamentos.

## 2. Repositorio

- Abra o GitHub e mostre o repositorio publico.
- Mostre o README, `scripts/script_bd.sql` e os scripts Azure CLI.
- Destaque que nao existem credenciais reais no codigo.

## 3. Clone obrigatorio

Comece os testes em um diretorio vazio e execute diante da camera:

```bash
git clone https://github.com/Challenge2026-2TDSPI/ChallengeAPI.git
cd ChallengeAPI
chmod +x scripts/*.sh
```

Nao utilize uma copia que ja esteja configurada.

## 4. Criacao dos recursos pela CLI

Execute os passos exatamente como documentados no README:

```bash
export RM=rm563304
export LOCATION=eastus
./scripts/01_create-resource-group.sh
./scripts/02_create-app-service.sh
read -r -s -p "Senha forte do Azure SQL: " SQL_ADMIN_PASSWORD
echo
export SQL_ADMIN_PASSWORD
./scripts/03_create-azure-sql.sh
./scripts/04_initialize-database.sh
```

Depois, abra o Resource Group no portal e mostre:

- App Service Plan;
- App Service;
- SQL Server;
- SQL Database.

## 5. Testes e deploy

No mesmo clone, execute:

```bash
./scripts/05_deploy-app-service.sh
```

Mostre a execucao dos testes e a confirmacao do ZIP deploy. Abra:

- `/health` para comprovar a conexao com o banco;
- `/swagger` para apresentar os endpoints;
- `/api/Tutores` e `/api/Pets` para mostrar os dados iniciais.

## 6. CRUD com persistencia - trecho sem cortes

Avise verbalmente que a partir desse ponto nao havera cortes. Execute:

```bash
./scripts/06_test-crud.sh
```

Explique cada bloco enquanto ele aparece:

1. Inclusao do tutor e `SELECT` que encontra o registro.
2. Inclusao do pet relacionado e `SELECT` que encontra o registro.
3. Alteracao do tutor e `SELECT` com o novo nome/telefone.
4. Alteracao do pet e `SELECT` com o novo nome/idade.
5. Consulta dos dois registros pela API.
6. Exclusao do pet e `SELECT` vazio.
7. Exclusao do tutor e `SELECT` vazio.

Para reforcar a evidencia, abra o Query Editor do Azure SQL e execute:

```sql
SELECT * FROM dbo.Tutores ORDER BY Id;
SELECT * FROM dbo.Pets ORDER BY Id;
SELECT * FROM dbo.Vacinas ORDER BY Id;
SELECT * FROM dbo.Consultas ORDER BY Id;
```

## 7. Encerramento

- Retome os beneficios para a clinica e os tutores.
- Mostre novamente o App Service funcionando fora do localhost.
- Informe que a API e o banco estao totalmente integrados na Azure.
- Mostre os integrantes e RMs.

Depois de salvar todas as evidencias e apenas quando o ambiente puder ser
removido, execute `./scripts/99_delete-azure.sh` fora do trecho principal.
