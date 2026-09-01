using ChallengeAPI.Controllers;
using ChallengeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace ChallengeAPI.UnitTests;

public class ConsultaControllerTests
{
    [Fact]
    public async Task GetConsulta_ShouldReturnOk_WhenConsultaExists()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create(nameof(GetConsulta_ShouldReturnOk_WhenConsultaExists));
        context.Consultas.Add(new Consulta
        {
            Id = 1,
            DataConsulta = "2026-08-20",
            Descricao = "Consulta de rotina",
            Veterinario = "Dr. Silva",
            PetId = 1
        });
        await context.SaveChangesAsync();
        var controller = new ConsultaController(context);

        // Act
        var result = await controller.GetConsulta(1);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal("Dr. Silva", Assert.IsType<Consulta>(ok.Value).Veterinario);
    }

    [Fact]
    public async Task GetConsulta_ShouldReturnNotFound_WhenConsultaDoesNotExist()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create(nameof(GetConsulta_ShouldReturnNotFound_WhenConsultaDoesNotExist));
        var controller = new ConsultaController(context);

        // Act
        var result = await controller.GetConsulta(999);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostConsulta_ShouldCreateConsulta()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create(nameof(PostConsulta_ShouldCreateConsulta));
        var controller = new ConsultaController(context);
        var consulta = new Consulta
        {
            DataConsulta = "2026-08-21",
            Descricao = "Vacinação",
            Veterinario = "Dra. Ana",
            PetId = 1
        };

        // Act
        var result = await controller.PostConsulta(consulta);

        // Assert
        Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(1, context.Consultas.Count());
    }

    [Fact]
    public async Task PutConsulta_ShouldReturnBadRequest_WhenIdsDoNotMatch()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create(nameof(PutConsulta_ShouldReturnBadRequest_WhenIdsDoNotMatch));
        var controller = new ConsultaController(context);
        var consulta = new Consulta { Id = 2, DataConsulta = "2026-08-21", Descricao = "Retorno", Veterinario = "Dra. Ana", PetId = 1 };

        // Act
        var result = await controller.PutConsulta(1, consulta);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
}
