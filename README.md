# CLYVO VET — ChallengeAPI

> API RESTful para gestão da jornada contínua de saúde do pet.

---

## Índice

- [Descrição do Projeto](#descrição-do-projeto)
- [Benefícios para o Negócio](#benefícios-para-o-negócio)
- [Arquitetura Macro](#arquitetura-macro)
- [Relacionamentos da Aplicação](#relacionamentos-da-aplicação)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Rotas da API](#rotas-da-api)
- [Como Instalar (How To)](#como-instalar-how-to)
- [Documentação OpenAPI](#documentação-openapi)
- [Dockerfile](#dockerfile)
- [Docker Compose](#docker-compose)
- [Script Azure CLI](#script-azure-cli)
- [Equipe](#equipe)
- [Disciplina](#disciplina)

---

## Descrição do Projeto

A CLYVO VET API é uma solução REST desenvolvida em .NET 8 com Oracle Database, criada para resolver a descontinuidade no cuidado preventivo de pets no Brasil.

O sistema permite gerenciar o histórico completo de saúde dos animais — tutores, pets, vacinas e consultas veterinárias — de forma contínua e estruturada, transformando a experiência reativa (apenas emergências) em um modelo preventivo e proativo.

---

## Benefícios para o Negócio

| Benefício | Impacto |
| Histórico longitudinal estruturado | Clínicas acessam todo o histórico do pet em um lugar |
| Aumento de recorrência | Vacinas e consultas registradas reduzem abandono de tratamentos |
| Maior LTV por pet | Acompanhamento preventivo gera mais visitas planejadas |
| Redução de emergências evitáveis | Protocolo preventivo diminui agravamentos desnecessários |
| Dados para decisão clínica | Histórico estruturado apoia diagnósticos mais precisos |

---

## Arquitetura Macro

```plaintext
┌──────────────┐    HTTP :8080   ┌──────────────────────────────────────┐
│   Usuário    │ ──────────────► │        Azure VM Linux Ubuntu         │
│ (Postman /   │                 │  ┌───────────────┐  ┌─────────────┐  │
│  Swagger /   │ ◄────────────── │  │  Container    │  │  Container  │  │
│  Scalar)     │    JSON         │  │  API .NET 8   │◄►│  Oracle XE  │  │
└──────────────┘                 │  │  porta 8080   │  │  porta 1521 │  │
                                 │  └───────────────┘  └─────────────┘  │
                                 │           └── Volume nomeado ─────────┘
                                 └──────────────────────────────────────┘
```

---

## Relacionamentos da Aplicação

```plaintext
Tutor
 └── Pets
      ├── Vacinas
      └── Consultas
```

### Relações implementadas

- Um Tutor pode possuir vários Pets
- Um Pet pertence a um Tutor
- Um Pet pode possuir várias Vacinas
- Um Pet pode possuir várias Consultas

As relações foram implementadas utilizando Entity Framework Core com Foreign Keys e Oracle Database.

---

## Tecnologias Utilizadas

- ASP.NET Core 8.0
- Entity Framework Core
- Oracle Database (containerizado)
- Oracle Entity Framework Core Provider
- RESTful API
- XML Documentation
- Swagger / OpenAPI
- Scalar
- Docker + Docker Compose
- Microsoft Azure (VM Linux)
- Azure CLI
- Git / GitHub

---

## Estrutura do Projeto

```plaintext
ChallengeAPI/
├── Controllers/
├── Data/
├── Models/
├── Migrations/
├── Properties/
├── docker/
│   ├── Dockerfile
│   └── docker-compose.yml
├── scripts/
│   ├── setup-azure.sh
│   └── delete-azure.sh
├── Program.cs
├── appsettings.json
└── README.md
```

---

## Rotas da API

### Tutores

| Método | Endpoint | Descrição | Status |
| `GET` | `/api/Tutores` | Lista todos os tutores | 200 |
| `GET` | `/api/Tutores/{id}` | Busca tutor por ID | 200 / 404 |
| `POST` | `/api/Tutores` | Cadastra novo tutor | 201 / 400 |
| `PUT` | `/api/Tutores/{id}` | Atualiza tutor | 204 / 404 |
| `DELETE` | `/api/Tutores/{id}` | Remove tutor | 204 / 404 |

---

### Pets

| Método | Endpoint | Descrição | Status |
| `GET` | `/api/Pets` | Lista todos os pets | 200 |
| `GET` | `/api/Pets/{id}` | Busca pet por ID | 200 / 404 |
| `GET` | `/api/Pets/nome/{nome}` | Busca pet por nome | 200 |
| `POST` | `/api/Pets` | Cadastra novo pet | 201 / 400 |
| `PUT` | `/api/Pets/{id}` | Atualiza pet | 204 / 404 |
| `DELETE` | `/api/Pets/{id}` | Remove pet | 204 / 404 |

---

### Vacinas

| Método | Endpoint | Descrição | Status |
| `GET` | `/api/Vacinas` | Lista todas as vacinas | 200 |
| `GET` | `/api/Vacinas/{id}` | Busca vacina por ID | 200 / 404 |
| `GET` | `/api/Vacinas/pet/{petId}` | Vacinas de um pet | 200 |
| `POST` | `/api/Vacinas` | Registra vacina | 201 / 400 |
| `PUT` | `/api/Vacinas/{id}` | Atualiza vacina | 204 / 404 |
| `DELETE` | `/api/Vacinas/{id}` | Remove vacina | 204 / 404 |

---

### Consultas

| Método | Endpoint | Descrição | Status |
| `GET` | `/api/Consultas` | Lista todas as consultas | 200 |
| `GET` | `/api/Consultas/{id}` | Busca consulta por ID | 200 / 404 |
| `GET` | `/api/Consultas/pet/{petId}` | Consultas de um pet | 200 |
| `GET` | `/api/Consultas/veterinario/{veterinario}` | Consultas por veterinário | 200 |
| `POST` | `/api/Consultas` | Registra consulta | 201 / 400 |
| `PUT` | `/api/Consultas/{id}` | Atualiza consulta | 204 / 404 |
| `DELETE` | `/api/Consultas/{id}` | Remove consulta | 204 / 404 |

---

## Como Instalar (How To)

### Pré-requisitos

- Docker Desktop instalado e rodando
- Git instalado
- Azure CLI instalado (para deploy em nuvem)

---

### 1. Clonar o repositório

```bash
git clone https://github.com/Challenge2026-2TDSPI/ChallengeAPI.git
cd ChallengeAPI
```

---

### 2. Rodar localmente com Docker

```bash
docker-compose -f docker/docker-compose.yml up -d --build
```

Aguarde alguns minutos para o Oracle inicializar.

Acesse:

- Swagger: http://localhost:8080/swagger
- Scalar: http://localhost:8080/scalar
- API: http://localhost:8080/api/Pets

---

### 3. Verificar containers

```bash
docker-compose -f docker/docker-compose.yml ps
```

---

### 4. Parar containers

```bash
docker-compose -f docker/docker-compose.yml down
```

---

### 5. Deploy na Azure

```bash
bash scripts/setup-azure.sh

ssh clyvovet@<IP_DA_VM>

git clone https://github.com/Challenge2026-2TDSPI/ChallengeAPI.git
cd ChallengeAPI

docker-compose -f docker/docker-compose.yml up -d --build

bash scripts/delete-azure.sh
```

---

## Documentação OpenAPI

A API possui documentação automática via Swagger e Scalar.

### Swagger UI

```plaintext
http://localhost:8080/swagger
```

### Scalar

```plaintext
http://localhost:8080/scalar
```

Todos os endpoints possuem:

- Descrição
- Responses HTTP
- Parâmetros documentados
- Modelos de requisição
- Estrutura OpenAPI

---

## Dockerfile

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ChallengeAPI.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish ChallengeAPI.csproj -c Release -o /out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

RUN addgroup --system appgroup && \
    adduser --system --ingroup appgroup --no-create-home appuser

COPY --from=build /out .

RUN chown -R appuser:appgroup /app

USER appuser

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "ChallengeAPI.dll"]
```

---

## Docker Compose

```yaml
services:
  oracle:
    image: gvenzl/oracle-xe:21-slim
    container_name: challengeapi-oracle

    environment:
      ORACLE_PASSWORD: Oracle123

    ports:
      - "1521:1521"

    volumes:
      - oracle_data:/opt/oracle/oradata

    healthcheck:
      test: ["CMD", "healthcheck.sh"]
      interval: 30s
      retries: 15

  api:
    build:
      context: ..
      dockerfile: docker/Dockerfile

    container_name: challengeapi-app

    ports:
      - "8080:8080"

    environment:
      - ConnectionStrings__OracleConnection=User Id=system;Password=Oracle123;Data Source=oracle:1521/XEPDB1

    depends_on:
      oracle:
        condition: service_healthy

volumes:
  oracle_data:
    name: challengeapi_oracle_data
```

---

## Script Azure CLI

```bash
az group create --name rg-challengeapi --location brazilsouth

az vm create \
  --resource-group rg-challengeapi \
  --name vm-challengeapi \
  --image Ubuntu2204 \
  --size Standard_B2s \
  --admin-username clyvovet \
  --generate-ssh-keys

az vm open-port \
  --resource-group rg-challengeapi \
  --name vm-challengeapi \
  --port 8080 \
  --priority 1001

az vm open-port \
  --resource-group rg-challengeapi \
  --name vm-challengeapi \
  --port 1521 \
  --priority 1002

az vm run-command invoke \
  --resource-group rg-challengeapi \
  --name vm-challengeapi \
  --command-id RunShellScript \
  --scripts "curl -fsSL https://get.docker.com | sh && apt-get install -y git nano"
```

Script completo disponível em:

```plaintext
scripts/setup-azure.sh
```

---
//Lembrando que as portas são únicas, rode o programa normal no Visual Studio e acesse pela sua porta disponível, a parte de docker é somente para cloud computing, mas a parte de .NET está rodando tranquilo, abra a solução no VS, rode e abra o scala ou swagger, porém com sua porta fornecida.

## Equipe
| Eduardo Augusto de Oliveira Souza | RM565269 |
| Fellipe Costa de Oliveira | RM564673 |
| Felype Ferreira Maschio | RM563009 |
| Gustavo Vieira de Matos | RM563304 |
| Pedro Henrique dos Santos Costa | RM562156 |

---

## Disciplina

FIAP — 2TDS  
Challenge Sprint 2026  
DevOps Tools & Cloud Computing
.NET

---

---

## Deploy — Azure + Docker (DevOps Sprint 1)

A aplicação está containerizada e rodando em uma VM Linux na Azure.

```bash
# Subir containers em background
docker compose -f docker/docker-compose.yml up -d --build

# Ver containers rodando
docker ps

# Reiniciar containers
docker compose -f docker/docker-compose.yml restart
```

**Configurações de deploy:**
- Porta externa: **80** → porta interna do container: **8080**
- Oracle XE na porta: **1521**
- Volume nomeado: `challengeapi_oracle_data`
- Usuário da aplicação: `appuser` (sem privilégios root)
- VM: AlmaLinux 10.1 — Standard_D2s_v3 — Chile Central

---

## Integrantes

| Nome | RM |
|---|---|
| Eduardo Augusto de Oliveira Souza | RM565269 |
| Fellipe Costa de Oliveira | RM564673 |
| Felype Ferreira Maschio | RM563009 |
| Gustavo Vieira de Matos | RM563304 |
| Pedro Henrique dos Santos Costa | RM562156 |

## Objetivo Acadêmico

Desenvolver uma API RESTful profissional utilizando ASP.NET Core, Oracle Database, Docker, Azure e documentação OpenAPI seguindo boas práticas de arquitetura e integração cloud-native.
````
