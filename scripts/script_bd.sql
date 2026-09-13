/*
  CLYVO VET - ChallengeAPI
  Banco: Azure SQL Database (PaaS)

  DDL comentado das tabelas centrais da aplicacao. As quatro tabelas formam
  o historico de saude do pet e possuem chaves primarias e estrangeiras.
  O script pode ser reexecutado: ele remove o schema anterior na ordem
  correta e recria cinco registros significativos por tabela.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

/* Limpeza em ordem inversa das dependencias. */
DROP TABLE IF EXISTS dbo.Consultas;
DROP TABLE IF EXISTS dbo.Vacinas;
DROP TABLE IF EXISTS dbo.Pets;
DROP TABLE IF EXISTS dbo.Tutores;
GO

/* Tutores: responsaveis legais pelos pets cadastrados. */
CREATE TABLE dbo.Tutores
(
    Id        INT IDENTITY(1,1) NOT NULL,
    Nome      NVARCHAR(150) NOT NULL,
    Telefone  NVARCHAR(20)  NOT NULL,
    Email     NVARCHAR(150) NOT NULL,
    CONSTRAINT PK_Tutores PRIMARY KEY (Id),
    CONSTRAINT UQ_Tutores_Email UNIQUE (Email)
);
GO

/* Pets: animais atendidos, sempre vinculados a um tutor. */
CREATE TABLE dbo.Pets
(
    Id         INT IDENTITY(1,1) NOT NULL,
    Nome       NVARCHAR(100) NOT NULL,
    Especie    NVARCHAR(50)  NOT NULL,
    Raca       NVARCHAR(100) NOT NULL,
    Idade      INT NOT NULL,
    TutorId    INT NOT NULL,
    ClinicaId  INT NOT NULL,
    CONSTRAINT PK_Pets PRIMARY KEY (Id),
    CONSTRAINT CK_Pets_Idade CHECK (Idade >= 0),
    CONSTRAINT FK_Pets_Tutores_TutorId
        FOREIGN KEY (TutorId) REFERENCES dbo.Tutores(Id) ON DELETE CASCADE
);
CREATE INDEX IX_Pets_TutorId ON dbo.Pets(TutorId);
GO

/* Vacinas: doses aplicadas e previsao do proximo reforco de cada pet. */
CREATE TABLE dbo.Vacinas
(
    Id             INT IDENTITY(1,1) NOT NULL,
    NomeVacina     NVARCHAR(120) NOT NULL,
    DataAplicacao  NVARCHAR(10)  NOT NULL,
    ProximaDose    NVARCHAR(10)  NOT NULL,
    PetId          INT NOT NULL,
    CONSTRAINT PK_Vacinas PRIMARY KEY (Id),
    CONSTRAINT FK_Vacinas_Pets_PetId
        FOREIGN KEY (PetId) REFERENCES dbo.Pets(Id) ON DELETE CASCADE
);
CREATE INDEX IX_Vacinas_PetId ON dbo.Vacinas(PetId);
GO

/* Consultas: atendimentos veterinarios registrados no historico do pet. */
CREATE TABLE dbo.Consultas
(
    Id             INT IDENTITY(1,1) NOT NULL,
    DataConsulta   NVARCHAR(10)  NOT NULL,
    Descricao      NVARCHAR(500) NOT NULL,
    Veterinario    NVARCHAR(150) NOT NULL,
    PetId          INT NOT NULL,
    CONSTRAINT PK_Consultas PRIMARY KEY (Id),
    CONSTRAINT FK_Consultas_Pets_PetId
        FOREIGN KEY (PetId) REFERENCES dbo.Pets(Id) ON DELETE CASCADE
);
CREATE INDEX IX_Consultas_PetId ON dbo.Consultas(PetId);
GO

/* Carga inicial: cinco linhas significativas em cada tabela. */
INSERT INTO dbo.Tutores (Nome, Telefone, Email) VALUES
    (N'Gustavo Vieira', N'11988887777', N'gustavo@clyvovet.com.br'),
    (N'Rafael Souza', N'11977776666', N'rafael@clyvovet.com.br'),
    (N'Marina Alves', N'11966665555', N'marina@clyvovet.com.br'),
    (N'Bruno Lima', N'11955554444', N'bruno@clyvovet.com.br'),
    (N'Carla Nogueira', N'11944443333', N'carla@clyvovet.com.br');

INSERT INTO dbo.Pets (Nome, Especie, Raca, Idade, TutorId, ClinicaId) VALUES
    (N'Thor', N'Cao', N'Labrador', 3, 1, 1),
    (N'Mimi', N'Gato', N'SRD', 2, 2, 1),
    (N'Bidu', N'Cao', N'Poodle', 5, 3, 1),
    (N'Luna', N'Gato', N'Siames', 1, 4, 1),
    (N'Rex', N'Cao', N'Bulldog', 4, 5, 1);

INSERT INTO dbo.Vacinas (NomeVacina, DataAplicacao, ProximaDose, PetId) VALUES
    (N'V10', N'2026-01-10', N'2027-01-10', 1),
    (N'Antirrabica', N'2026-02-15', N'2027-02-15', 2),
    (N'Giardia', N'2026-03-05', N'2026-09-05', 3),
    (N'V4', N'2026-04-20', N'2027-04-20', 4),
    (N'Antirrabica', N'2026-05-01', N'2027-05-01', 5);

INSERT INTO dbo.Consultas (DataConsulta, Descricao, Veterinario, PetId) VALUES
    (N'2026-06-01', N'Checkup de rotina', N'Dra. Ana Paula', 1),
    (N'2026-06-10', N'Vomito e falta de apetite', N'Dr. Marcos Vinicius', 2),
    (N'2026-07-02', N'Avaliacao dermatologica', N'Dra. Ana Paula', 3),
    (N'2026-07-18', N'Consulta pos-vacina', N'Dr. Marcos Vinicius', 4),
    (N'2026-08-22', N'Avaliacao cardiologica', N'Dra. Ana Paula', 5);
GO

/* Verificacao da carga inicial. */
SELECT 'Tutores' AS Tabela, COUNT(*) AS Quantidade FROM dbo.Tutores
UNION ALL SELECT 'Pets', COUNT(*) FROM dbo.Pets
UNION ALL SELECT 'Vacinas', COUNT(*) FROM dbo.Vacinas
UNION ALL SELECT 'Consultas', COUNT(*) FROM dbo.Consultas;
GO
