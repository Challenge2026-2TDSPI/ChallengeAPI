# ChallengeAPI — CLYVO VET 🐾

API RESTful desenvolvida em ASP.NET Core para gerenciamento de tutores, pets, vacinas e consultas veterinárias.

Projeto desenvolvido para o Challenge Sprint 1 da FIAP — Disciplina DevOps Tools & Cloud Computing — Turma 2TDSPI.

---

## Tecnologias Utilizadas

- ASP.NET Core Web API (.NET 8)
- Entity Framework Core
- Oracle Database XE 21c
- Docker + Docker Compose
- Azure Virtual Machine (AlmaLinux 10.1)
- Swagger / OpenAPI
- GitHub

---

## Estrutura do Projeto
Controllers/
Data/
Models/
Migrations/
docker/
Dockerfile
docker-compose.yml

---

## Entidades da API

| Entidade | Descrição |
|---|---|
| Tutor | Responsável pelos pets cadastrados |
| Pet | Animal cadastrado no sistema |
| Vacina | Vacinas aplicadas nos pets |
| Consulta | Consultas veterinárias realizadas |

---

## Endpoints

### Tutores
| Método | Endpoint |
|---|---|
| GET | /api/Tutores |
| GET | /api/Tutores/{id} |
| GET | /api/Tutores/nome/{nome} |
| GET | /api/Tutores/email/{email} |
| GET | /api/Tutores/telefone/{telefone} |
| POST | /api/Tutores |
| PUT | /api/Tutores/{id} |
| DELETE | /api/Tutores/{id} |

### Pets
| Método | Endpoint |
|---|---|
| GET | /api/Pets |
| GET | /api/Pets/{id} |
| GET | /api/Pets/nome/{nome} |
| GET | /api/Pets/especie/{especie} |
| GET | /api/Pets/raca/{raca} |
| GET | /api/Pets/idade/{idade} |
| POST | /api/Pets |
| PUT | /api/Pets/{id} |
| DELETE | /api/Pets/{id} |

### Vacinas
| Método | Endpoint |
|---|---|
| GET | /api/Vacinas |
| GET | /api/Vacinas/{id} |
| GET | /api/Vacinas/nome/{nome} |
| GET | /api/Vacinas/data-aplicacao/{data} |
| GET | /api/Vacinas/proxima-dose/{data} |
| POST | /api/Vacinas |
| PUT | /api/Vacinas/{id} |
| DELETE | /api/Vacinas/{id} |

### Consultas
| Método | Endpoint |
|---|---|
| GET | /api/Consulta |
| GET | /api/Consulta/{id} |
| GET | /api/Consulta/veterinario/{veterinario} |
| GET | /api/Consulta/pet/{petId} |
| POST | /api/Consulta |
| PUT | /api/Consulta/{id} |
| DELETE | /api/Consulta/{id} |

---

## Deploy — Azure + Docker

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
- API disponível em: `http://57.156.58.199/swagger`
- Porta externa: **80** → porta interna do container: **8080**
- Oracle XE na porta: **1521**
- Volume nomeado: `challengeapi_oracle_data`
- Usuário da aplicação: `appuser` (sem privilégios root)

---

## Como executar localmente

```bash
# 1. Clonar o repositório
git clone https://github.com/Challenge2026-2TDSPI/ChallengeAPI.git

# 2. Restaurar pacotes
dotnet restore

# 3. Configurar a connection string no appsettings.json
# "OracleConnection": "User Id=...;Password=...;Data Source=..."

# 4. Executar as migrations
dotnet ef database update

# 5. Executar a aplicação
dotnet run
```

**Documentação disponível em:**
- Swagger: `/swagger`
- Scalar: `/scalar`

---

## Integrantes

| Nome | RM |
|---|---|
| Eduardo Augusto de Oliveira Souza | RM565269 |
| Fellipe Costa de Oliveira | RM564673 |
| Felype Ferreira Maschio | RM563009 |
| Gustavo Vieira de Matos | RM563304 |
| Pedro Henrique dos Santos Costa | RM562156 |
