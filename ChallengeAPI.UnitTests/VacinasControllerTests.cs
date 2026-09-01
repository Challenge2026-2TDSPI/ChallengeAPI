using ChallengeAPI.Controllers;
using ChallengeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace ChallengeAPI.UnitTests;

public class VacinasControllerTests
{
    [Fact]
    public async Task GetVacinaById_ShouldReturnOk_WhenVacinaExists()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create(nameof(GetVacinaById_ShouldReturnOk_WhenVacinaExists));
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
        context.Vacinas.Add(new Vacina
        {
            Id = 1,
            NomeVacina = "V10",
            DataAplicacao = "2026-08-01",
            ProximaDose = "2027-08-01",
            PetId = 1
        });
        await context.SaveChangesAsync();
        var controller = new VacinasController(context);

        // Act
        var result = await controller.GetVacinaById(1);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal("V10", Assert.IsType<Vacina>(ok.Value).NomeVacina);
    }//é um comportamento padrão do EF Core InMemory. Quando a chave estrangeira (FK) é obrigatória (não-anulável), o .Include() funciona como um Inner Join (descarta a linha se o par não existir). Se fosse opcional (anulável), funcionaria como um Left Join. O erro não estava no código de produção, mas sim no teste que foi alimentado com dados incompletos.

    [Fact]
    public async Task PostVacina_ShouldCreateVacina()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create(nameof(PostVacina_ShouldCreateVacina));
        var controller = new VacinasController(context);
        var vacina = new Vacina
        {
            NomeVacina = "Antirrábica",
            DataAplicacao = "2026-08-10",
            ProximaDose = "2027-08-10",
            PetId = 1
        };

        // Act
        var result = await controller.PostVacina(vacina);

        // Assert
        Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(1, context.Vacinas.Count());
    }

    [Fact]
    public async Task DeleteVacina_ShouldReturnNotFound_WhenVacinaDoesNotExist()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create(nameof(DeleteVacina_ShouldReturnNotFound_WhenVacinaDoesNotExist));
        var controller = new VacinasController(context);

        // Act
        var result = await controller.DeleteVacina(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
