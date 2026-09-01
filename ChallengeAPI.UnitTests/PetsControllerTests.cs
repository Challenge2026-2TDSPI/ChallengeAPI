using ChallengeAPI.Controllers;
using ChallengeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ChallengeAPI.UnitTests;

public class PetsControllerTests
{
    [Fact]
    public async Task GetPetById_ShouldReturnPet_WhenPetExists()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create(nameof(GetPetById_ShouldReturnPet_WhenPetExists));
        context.Tutores.Add(new Tutor
        {
            Id = 1,
            Nome = "Eduardo",
            Telefone = "11999999999",
            Email = "eduardo@email.com"
        });
        context.Pets.Add(new Pet
        {
            Id = 1,
            Nome = "Rex",
            Especie = "Cachorro",
            Raca = "Labrador",
            Idade = 4,
            TutorId = 1,
            ClinicaId = 1
        });
        await context.SaveChangesAsync();
        var logger = new Mock<ILogger<PetsController>>();
        var controller = new PetsController(context, logger.Object);

        // Act
        var result = await controller.GetPetById(1);

        // Assert
        var returned = Assert.IsType<Pet>(result.Value);
        Assert.Equal("Rex", returned.Nome);
    } //Para corrigir o erro, foi necessário cadastrar o Tutor no banco de dados de teste antes de salvar o Pet, garantindo que o vínculo entre as duas entidades fosse válido durante a execução do teste.

    [Fact]
    public async Task GetPetById_ShouldReturnNotFoundAndLogWarning_WhenPetDoesNotExist()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create(nameof(GetPetById_ShouldReturnNotFoundAndLogWarning_WhenPetDoesNotExist));
        var logger = new Mock<ILogger<PetsController>>();
        var controller = new PetsController(context, logger.Object);

        // Act
        var result = await controller.GetPetById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
        logger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((state, _) => state.ToString()!.Contains("Pet com ID 999 não encontrado")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task PostPet_ShouldCreatePet()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create(nameof(PostPet_ShouldCreatePet));
        var logger = new Mock<ILogger<PetsController>>();
        var controller = new PetsController(context, logger.Object);
        var pet = new Pet
        {
            Nome = "Mel",
            Especie = "Gato",
            Raca = "SRD",
            Idade = 2,
            TutorId = 1,
            ClinicaId = 1
        };

        // Act
        var result = await controller.PostPet(pet);

        // Assert
        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(1, context.Pets.Count());
        Assert.Equal("Mel", ((Pet)created.Value!).Nome);
    }

    [Fact]
    public async Task PutPet_ShouldReturnBadRequest_WhenIdsDoNotMatch()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create(nameof(PutPet_ShouldReturnBadRequest_WhenIdsDoNotMatch));
        var logger = new Mock<ILogger<PetsController>>();
        var controller = new PetsController(context, logger.Object);
        var pet = new Pet { Id = 2, Nome = "Mel", Especie = "Gato", Raca = "SRD", Idade = 2, TutorId = 1, ClinicaId = 1 };

        // Act
        var result = await controller.PutPet(1, pet);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
}
