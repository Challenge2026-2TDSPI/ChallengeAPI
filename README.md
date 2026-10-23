# CLYVO VET — ChallengeAPI

> API RESTful para gestão da jornada contínua de saúde do pet, desenvolvida em ASP.NET Core 8, com persistência em Oracle Database e MongoDB, autenticação JWT e observabilidade.

---

## Índice

- [Descrição do Projeto](#descrição-do-projeto)
- [Benefícios para o Negócio](#benefícios-para-o-negócio)
- [Funcionalidades](#funcionalidades)
- [Arquitetura Macro](#arquitetura-macro)
- [Relacionamentos da Aplicação](#relacionamentos-da-aplicação)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Rotas da API](#rotas-da-api)
- [Autenticação JWT](#autenticação-jwt)
- [Health Check](#health-check)
- [Logging e Observabilidade](#logging-e-observabilidade)
- [Como Instalar e Executar](#como-instalar-e-executar)
- [Configuração de Segredos](#configuração-de-segredos)
- [Documentação OpenAPI](#documentação-openapi)
- [Testes Automatizados](#testes-automatizados)
- [Docker e Azure](#docker-e-azure)
- [Equipe](#equipe)
- [Disciplina e Objetivo Acadêmico](#disciplina-e-objetivo-acadêmico)

---

## Descrição do Projeto

A **CLYVO VET API** é uma solução REST desenvolvida em .NET 8 para apoiar a gestão contínua da saúde de animais de estimação.

O sistema permite organizar informações de tutores, pets, vacinas e consultas veterinárias, estruturando o histórico de saúde dos animais e facilitando o acompanhamento preventivo.

Ao longo das sprints, a aplicação recebeu funcionalidades de persistência relacional, integração com banco de dados NoSQL, autenticação, documentação interativa, monitoramento de saúde e observabilidade.

## Benefícios para o Negócio

| Benefício | Impacto |
|---|---|
| Histórico estruturado | Centraliza informações relevantes sobre a saúde dos pets. |
| Acompanhamento preventivo | Facilita o registro de vacinas e consultas. |
| Continuidade do cuidado | Ajuda a manter o histórico do animal organizado. |
| Apoio à decisão clínica | Disponibiliza informações estruturadas para consulta. |
| Organização operacional | Permite gerenciar tutores, pets e registros veterinários por meio de uma API. |

---

## Funcionalidades

### Gestão de dados

- Cadastro, consulta, atualização e remoção de registros por meio de endpoints REST.
- Gerenciamento de tutores e pets.
- Registro de vacinas e consultas veterinárias.
- Persistência relacional utilizando Oracle Database e Entity Framework Core.

### Autenticação e autorização

- Autenticação baseada em JWT.
- Emissão e validação de tokens.
- Proteção de endpoints com autorização.
- Integração do esquema Bearer com o Swagger UI.

### Persistência NoSQL

- Integração com MongoDB Atlas.
- Configuração por meio de `MongoDbSettings`.
- Endpoint de teste de conectividade em `/api/MongoTest`.

### Monitoramento e observabilidade

- Health Check em `/health`.
- Logging estruturado com Serilog.
- Tracing e métricas com OpenTelemetry.
- Exportação de dados de observabilidade para o console durante a execução local.

---

## Arquitetura Macro

A aplicação é organizada em uma API ASP.NET Core, com controladores responsáveis pelas requisições HTTP, serviços para funcionalidades específicas e repositórios para acesso aos dados quando aplicável.

```text
                    Cliente HTTP
             Swagger / Scalar / Postman
                         |
                         v
              +----------------------+
              |   ASP.NET Core 8     |
              |     ChallengeAPI     |
              +----------------------+
                         |
             +-----------+-----------+
             |           |           |
             v           v           v
         Controllers  Services   Observability
             |                       |
             v                       v
        Entity Framework          Serilog
             |                    OpenTelemetry
             v
       Oracle Database

        MongoDB.Driver
             |
             v
        MongoDB Atlas
```

A infraestrutura com Docker e Azure faz parte do contexto de DevOps e Cloud Computing. Para desenvolvimento e validação local da API .NET, utilize a execução pelo Visual Studio ou pelo terminal.

## Relacionamentos da Aplicação

```text
Tutor
 └── Pets
      ├── Vacinas
      └── Consultas
```

### Relações implementadas

- Um tutor pode possuir vários pets.
- Um pet pertence a um tutor.
- Um pet pode possuir várias vacinas.
- Um pet pode possuir várias consultas.

As relações do modelo relacional são implementadas com Entity Framework Core e Oracle Database.

---

## Tecnologias Utilizadas

| Tecnologia | Finalidade |
|---|---|
| C# / ASP.NET Core 8 | Desenvolvimento da API REST |
| Entity Framework Core | Mapeamento objeto-relacional e persistência |
| Oracle Database | Persistência relacional |
| Oracle Entity Framework Core Provider | Integração do EF Core com Oracle |
| MongoDB.Driver | Acesso ao MongoDB |
| MongoDB Atlas | Banco de dados NoSQL |
| JWT / Bearer Authentication | Autenticação e autorização |
| Swagger / OpenAPI | Documentação e testes dos endpoints |
| Scalar | Interface alternativa para explorar a API |
| Serilog | Logging estruturado |
| OpenTelemetry | Tracing e métricas |
| xUnit / Moq | Ferramentas de testes automatizados |
| EF Core InMemory | Persistência em memória para testes |
| Docker / Docker Compose | Containerização e orquestração |
| Microsoft Azure / Azure CLI | Infraestrutura em nuvem |
| Git / GitHub | Versionamento e colaboração |

---

## Estrutura do Projeto

A estrutura pode variar conforme os arquivos e projetos de teste presentes na solução.

```text
ChallengeAPI/
├── Controllers/
│   ├── AuthController.cs
│   ├── MongoTestController.cs
│   ├── ConsultasController.cs
│   ├── PetsController.cs
│   ├── TutoresController.cs
│   └── VacinasController.cs
├── Data/
│   └── AppDbContext.cs
├── Models/
├── Repositories/
├── Services/
│   └── TokenService.cs
├── Settings/
├── SettingsJWT/
├── Telemetry/
├── Properties/
├── ChallengeAPI.IntegrationTests/
├── ChallengeAPI.UnitTests/
├── Program.cs
├── ChallengeAPI.csproj
└── README.md
```

> A lista é ilustrativa: mantenha no repositório os nomes e diretórios que realmente existem na sua solução.

---

## Rotas da API

As rotas abaixo refletem os endpoints documentados no projeto. Os códigos HTTP indicados são os resultados esperados para operações bem-sucedidas ou situações comuns de validação; a resposta efetiva depende da requisição e das regras de cada controlador.

### Tutores

| Método | Endpoint | Descrição |
|---|---|---|
| GET | `/api/Tutores` | Lista os tutores |
| GET | `/api/Tutores/{id}` | Busca um tutor por ID |
| POST | `/api/Tutores` | Cadastra um tutor |
| PUT | `/api/Tutores/{id}` | Atualiza um tutor |
| DELETE | `/api/Tutores/{id}` | Remove um tutor |

### Pets

| Método | Endpoint | Descrição |
|---|---|---|
| GET | `/api/Pets` | Lista os pets |
| GET | `/api/Pets/{id}` | Busca um pet por ID |
| GET | `/api/Pets/nome/{nome}` | Busca pets por nome |
| POST | `/api/Pets` | Cadastra um pet |
| PUT | `/api/Pets/{id}` | Atualiza um pet |
| DELETE | `/api/Pets/{id}` | Remove um pet |

### Vacinas

| Método | Endpoint | Descrição |
|---|---|---|
| GET | `/api/Vacinas` | Lista as vacinas |
| GET | `/api/Vacinas/{id}` | Busca uma vacina por ID |
| GET | `/api/Vacinas/pet/{petId}` | Lista as vacinas de um pet |
| POST | `/api/Vacinas` | Registra uma vacina |
| PUT | `/api/Vacinas/{id}` | Atualiza uma vacina |
| DELETE | `/api/Vacinas/{id}` | Remove uma vacina |

### Consultas

| Método | Endpoint | Descrição |
|---|---|---|
| GET | `/api/Consultas` | Lista as consultas |
| GET | `/api/Consultas/{id}` | Busca uma consulta por ID |
| GET | `/api/Consultas/pet/{petId}` | Lista consultas de um pet |
| GET | `/api/Consultas/veterinario/{veterinario}` | Busca consultas por veterinário |
| POST | `/api/Consultas` | Registra uma consulta |
| PUT | `/api/Consultas/{id}` | Atualiza uma consulta |
| DELETE | `/api/Consultas/{id}` | Remove uma consulta |

### Autenticação e monitoramento

| Método | Endpoint | Descrição |
|---|---|---|
| POST | `/api/Auth/login` | Autentica o usuário e retorna um JWT |
| GET | `/api/Auth/dados-protegidos` | Exemplo de endpoint protegido por JWT |
| GET | `/api/MongoTest` | Verifica a conectividade com MongoDB |
| GET | `/health` | Verifica a saúde dos componentes monitorados |

Consulte o Swagger ou o Scalar para visualizar os parâmetros, os modelos de requisição e as respostas efetivamente documentadas.

---

## Autenticação JWT

A API utiliza autenticação Bearer com JSON Web Tokens (JWT).

### Fluxo de autenticação

1. Envie uma requisição `POST` para `/api/Auth/login` com as credenciais esperadas pelo controlador.
2. Copie o token JWT retornado.
3. No Swagger UI, clique em **Authorize**.
4. Informe o token conforme o esquema Bearer apresentado na interface.
5. Execute um endpoint protegido, como `/api/Auth/dados-protegidos`.

A API valida o token utilizando os parâmetros configurados para chave de assinatura, emissor, audiência e validade.

### Segurança

- Não publique chaves JWT, senhas ou tokens no repositório.
- Não inclua credenciais reais em `appsettings.json`, exemplos de código ou documentação pública.
- Utilize User Secrets no desenvolvimento local.
- Em produção, utilize um mecanismo seguro de configuração de credenciais.

---

## Health Check

A API disponibiliza o endpoint:

```http
GET /health
```

O Health Check verifica a disponibilidade do Oracle Database por meio do pacote `AspNetCore.HealthChecks.Oracle`.

A resposta apresenta o estado geral e os resultados das verificações registradas.

Exemplo de resposta saudável:

```json
{
  "status": "Healthy",
  "checks": [
    {
      "name": "oracle-database",
      "status": "Healthy",
      "description": null
    }
  ]
}
```

O resultado depende da disponibilidade do banco e das credenciais configuradas. Quando uma dependência monitorada está indisponível, o Health Check pode retornar `Unhealthy`.

---

## Logging e Observabilidade

### Serilog

A aplicação utiliza Serilog para registrar eventos de execução e requisições HTTP.

Os destinos configurados são:

- Console.
- Arquivos diários no diretório `logs/`, com nomes no formato `log-AAAA-MM-DD.txt`, conforme a configuração de rotação utilizada.

O middleware `UseSerilogRequestLogging()` registra informações das requisições, como método, caminho, status HTTP e duração.

O contexto de logging também é enriquecido com identificadores de tracing quando uma atividade está disponível.

### OpenTelemetry

A instrumentação inclui:

- Tracing das requisições ASP.NET Core.
- Tracing de chamadas HTTP feitas por `HttpClient`.
- Métricas HTTP.
- Métricas customizadas da aplicação.
- Exportação para o console durante a execução local.

O projeto utiliza instrumentos customizados identificados por `AppMetrics` e a fonte de tracing `ChallengeAPI`.

Esses recursos ajudam a investigar erros e acompanhar o comportamento da aplicação durante a execução.

---

## Como Instalar e Executar

### Pré-requisitos

- .NET SDK 8.
- Git.
- Visual Studio com suporte a ASP.NET e desenvolvimento web, ou VS Code com o SDK instalado.
- Acesso ao Oracle Database utilizado pelo projeto.
- Acesso ao MongoDB Atlas, caso sejam testadas as funcionalidades NoSQL.

Docker e Azure são necessários somente para os respectivos cenários de containerização e implantação.

### 1. Clonar o repositório

```bash
git clone https://github.com/Challenge2026-2TDSPI/ChallengeAPI.git
cd ChallengeAPI
```

### 2. Restaurar as dependências

Na pasta que contém `ChallengeAPI.csproj`, execute:

```bash
dotnet restore
dotnet build
```

### 3. Configurar as credenciais

Configure as strings de conexão e os parâmetros JWT por User Secrets, conforme a seção [Configuração de Segredos](#configuração-de-segredos).

Verifique se o Oracle e o MongoDB estão acessíveis antes de testar os respectivos endpoints.

### 4. Executar pelo Visual Studio

1. Abra a solução no Visual Studio.
2. Selecione `ChallengeAPI` como projeto de inicialização.
3. Execute a aplicação.
4. Consulte a URL e a porta apresentadas pelo Visual Studio.

A porta pode variar conforme o perfil de execução. Utilize o endereço exibido na sua máquina.

### 5. Executar pelo terminal

Dentro da pasta da API:

```bash
dotnet run
```

Acesse os endereços correspondentes à porta indicada no terminal.

Exemplo, caso a aplicação esteja rodando em `http://localhost:5241`:

- Swagger: `http://localhost:5241/swagger`
- Health Check: `http://localhost:5241/health`
- Teste MongoDB: `http://localhost:5241/api/MongoTest`

### 6. Encerrar a aplicação

No terminal em que a API está rodando, pressione `Ctrl + C`.

---

## Configuração de Segredos

As credenciais são específicas de cada ambiente e não devem ser incluídas no repositório.

### Oracle Database

Configure a chave `ConnectionStrings:OracleConnection`:

```powershell
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:OracleConnection" "User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=SEU_DATA_SOURCE"
```

Substitua os valores pelos dados reais do banco utilizado no ambiente local.

### MongoDB Atlas

Configure as chaves utilizadas por `MongoDbSettings`:

```powershell
dotnet user-secrets set "MongoDbSettings:ConnectionString" "SUA_CONNECTION_STRING_MONGODB"
dotnet user-secrets set "MongoDbSettings:DatabaseName" "NOME_DO_BANCO"
```

Utilize a URI real fornecida pelo MongoDB Atlas, com usuário e senha válidos. Verifique também as permissões do usuário e as regras de acesso à rede do cluster.

### JWT

Configure os parâmetros utilizados pela aplicação:

```powershell
dotnet user-secrets set "Jwt:SecretKey" "SUA_CHAVE_SECRETA"
dotnet user-secrets set "Jwt:Issuer" "ChallengeAPI"
dotnet user-secrets set "Jwt:Audience" "ChallengeAPI"
```

A chave deve atender aos requisitos de assinatura definidos pela implementação. Os valores de emissor e audiência devem corresponder à configuração usada para emitir e validar os tokens.

> Não substitua credenciais já funcionais sem necessidade. Os exemplos acima servem para configurar um novo ambiente; use os valores apropriados ao projeto.

Para conferir quais chaves estão configuradas:

```powershell
dotnet user-secrets list
```

Não compartilhe a saída completa desse comando quando ela contiver valores confidenciais.

---

## Documentação OpenAPI

A API utiliza Swagger/OpenAPI e Scalar para explorar e testar os endpoints.

### Swagger UI

```text
http://localhost:<porta>/swagger
```

### Scalar

```text
http://localhost:<porta>/scalar/v1
```

Substitua `<porta>` pela porta exibida durante a execução da API.

O Swagger permite consultar as rotas documentadas, visualizar modelos de requisição e resposta e utilizar o botão **Authorize** para testar endpoints protegidos por JWT.

O documento OpenAPI é disponibilizado em:

```text
http://localhost:<porta>/swagger/v1/swagger.json
```

---

## Testes Automatizados

A solução possui projetos destinados a testes unitários e de integração:

```text
ChallengeAPI.UnitTests/
ChallengeAPI.IntegrationTests/
```

### Testes unitários

O projeto utiliza ferramentas como:

- xUnit.
- Moq.
- EF Core InMemory, quando aplicável.
- Padrão AAA (Arrange, Act, Assert).

Esses testes permitem avaliar comportamentos específicos sem depender necessariamente de um banco de dados real.

### Testes de integração

O projeto utiliza ferramentas como:

- xUnit.
- `WebApplicationFactory`.
- `ICollectionFixture`.
- EF Core InMemory, conforme a configuração dos testes.

Os testes de integração permitem executar a aplicação em ambiente de teste e realizar requisições HTTP contra os endpoints.

### Executar os testes

Na raiz da solução:

```bash
dotnet restore
dotnet build
dotnet test
```

Os testes também podem ser executados pelo **Test Explorer** do Visual Studio.

---

## Docker e Azure

A containerização e a infraestrutura em nuvem fazem parte do contexto de DevOps e Cloud Computing do projeto. A execução local habitual da API .NET pode ser feita pelo Visual Studio, independentemente de um deploy em nuvem.

### Docker Compose

Caso os arquivos de Docker estejam disponíveis no repositório, a inicialização do ambiente containerizado pode ser realizada com:

```bash
docker compose -f docker/docker-compose.yml up -d --build
```

Verifique os containers:

```bash
docker compose -f docker/docker-compose.yml ps
```

Consulte os logs:

```bash
docker compose -f docker/docker-compose.yml logs
```

Para encerrar os containers:

```bash
docker compose -f docker/docker-compose.yml down
```

Antes de executar esse cenário, confira as variáveis de ambiente e as strings de conexão no arquivo Compose. Não utilize credenciais de exemplo em ambientes públicos ou de produção.

### Dockerfile

O Dockerfile do projeto utiliza uma imagem do SDK .NET para compilar a aplicação e uma imagem ASP.NET para executá-la, seguindo uma abordagem de build em múltiplos estágios.

O container da API utiliza a porta interna `8080` quando configurado conforme o Dockerfile documentado.

### Azure CLI

Os scripts de infraestrutura ficam no diretório `scripts/`, quando presentes no repositório:

```text
scripts/setup-azure.sh
scripts/delete-azure.sh
```

O script de provisionamento pode criar recursos de infraestrutura na Azure, dependendo de sua implementação e das permissões disponíveis.

**Atenção:** comandos de criação e remoção de recursos na nuvem podem gerar custos ou excluir recursos. Revise os scripts e confirme o grupo de recursos antes de executá-los.

A documentação de Docker e Azure descreve o cenário de infraestrutura; ela não significa, por si só, que o deploy esteja atualmente ativo ou disponível publicamente.

---

## Equipe

| Nome | RM |
|---|---|
| Eduardo Augusto de Oliveira Souza | RM565269 |
| Fellipe Costa de Oliveira | RM564673 |
| Felype Ferreira Maschio | RM563009 |
| Gustavo Vieira de Matos | RM563304 |
| Pedro Henrique dos Santos Costa | RM562156 |

---

## Disciplina e Objetivo Acadêmico

**Instituição:** FIAP  
**Turma:** 2TDS  
**Projeto:** Challenge Sprint 2026  
**Disciplinas:** .NET e DevOps Tools & Cloud Computing

O objetivo acadêmico é desenvolver uma API RESTful utilizando ASP.NET Core, persistência de dados, autenticação, documentação OpenAPI, monitoramento, observabilidade, testes automatizados e integração com ferramentas de infraestrutura e nuvem.

---

## Sprint 4 — Autenticação, MongoDB e Integração

Nesta etapa, o projeto foi ampliado com recursos de autenticação e integração com banco NoSQL, preservando a estrutura da API e os recursos implementados anteriormente.

### Autenticação JWT

- Emissão de tokens JWT.
- Validação de assinatura, emissor, audiência e validade.
- Proteção de endpoints com autorização.
- Configuração do esquema Bearer no Swagger UI.
- Testes de autenticação e acesso a endpoints protegidos.

### MongoDB Atlas

- Configuração da conexão por `MongoDbSettings`.
- Registro do cliente MongoDB por injeção de dependência.
- Integração com repositório para as operações previstas.
- Endpoint dedicado para teste de conectividade.

### Oracle e Health Check

- Persistência relacional com Oracle Database e Entity Framework Core.
- Verificação da conectividade do Oracle pelo endpoint `/health`.
- Resposta de saúde com status geral e resultado das verificações registradas.

### Validação final

Durante a validação local relatada pela equipe, foram confirmados:

- Build da aplicação concluído.
- Swagger e autorização JWT testados.
- Conexão com MongoDB Atlas funcionando.
- Conexão com Oracle funcionando.
- Health Check retornando `Healthy`.

