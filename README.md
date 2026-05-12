# Challenge API - Clínica Veterinária

API RESTful desenvolvida em ASP.NET Core para gerenciamento de tutores, pets, vacinas e consultas veterinárias.

Projeto desenvolvido para o Challenge Sprint da FIAP utilizando Oracle Database, Entity Framework Core, Swagger e Scalar.

---

# Tecnologias Utilizadas

- ASP.NET Core
- Entity Framework Core
- Oracle Database
- Swagger / OpenAPI
- Scalar
- C#
- REST API

---

# Estrutura do Projeto

```plaintext
Controllers/
Data/
Models/
Migrations/
Properties/
```

---

# Entidades da API

## Tutor
Responsável pelos pets cadastrados.

## Pet
Animal cadastrado no sistema.

## Vacina
Vacinas aplicadas nos pets.

## Consulta
Consultas veterinárias realizadas.

---

# Funcionalidades

CRUD completo de Tutores  
CRUD completo de Pets  
CRUD completo de Vacinas  
CRUD completo de Consultas  

GETs parametrizados  
Relacionamentos entre entidades  
Integração com Oracle Database  
Migrations com EF Core  
Documentação OpenAPI  

---

# Endpoints Principais

## Tutor

| Método | Endpoint |
|---|---|
| GET | /api/Tutores |
| GET | /api/Tutores/{id} |
| POST | /api/Tutores |
| PUT | /api/Tutores/{id} |
| DELETE | /api/Tutores/{id} |

---

## Pet

| Método | Endpoint |
|---|---|
| GET | /api/Pets |
| GET | /api/Pets/{id} |
| GET | /api/Pets/nome/{nome} |
| POST | /api/Pets |
| PUT | /api/Pets/{id} |
| DELETE | /api/Pets/{id} |

---

## Vacina

| Método | Endpoint |
|---|---|
| GET | /api/Vacinas |
| GET | /api/Vacinas/{id} |
| GET | /api/Vacinas/pet/{petId} |
| POST | /api/Vacinas |
| PUT | /api/Vacinas/{id} |
| DELETE | /api/Vacinas/{id} |

---

## Consulta

| Método | Endpoint |
|---|---|
| GET | /api/Consultas |
| GET | /api/Consultas/{id} |
| GET | /api/Consultas/pet/{petId} |
| GET | /api/Consultas/veterinario/{veterinario} |
| POST | /api/Consultas |
| PUT | /api/Consultas/{id} |
| DELETE | /api/Consultas/{id} |

---

# Como Executar o Projeto

## 1️. Clonar o repositório

```bash
git clone https://github.com/Challenge2026-2TDSPI/ChallengeAPI.git
```

---

## 2️. Instalar os pacotes

```bash
dotnet restore
```

---

## 3️. Configurar conexão Oracle

No arquivo:

```plaintext
appsettings.json
```

Configurar:

```json
"ConnectionStrings": {
  "OracleConnection": "User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/ORCL" //Nesse caso pode usar o meu mesmo
}
```

---

## 4️. Executar as migrations

```bash
dotnet ef database update
```

---

## 5️. Executar a aplicação

```bash
dotnet run
```

---

# Documentação

## Swagger

```plaintext
/swagger
```

## Scalar

```plaintext
/scalar
```

---

# Desenvolvido por

Eduardo Augusto de Oliveira Souza - RM 565269