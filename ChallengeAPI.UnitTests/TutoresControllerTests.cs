using ChallengeAPI.Controllers;
using ChallengeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ChallengeAPI.UnitTests;

public class TutoresControllerTests
{
    [Fact]
    public async Task GetTutorById_ShouldReturnOk_WhenTutorExists()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create(nameof(GetTutorById_ShouldReturnOk_WhenTutorExists));
        var tutor = new Tutor { Id = 1, Nome = "Eduardo", Telefone = "11999999999", Email = "eduardo@email.com" };
        context.Tutores.Add(tutor);
        await context.SaveChangesAsync();
        var controller = new TutoresController(context);

        // Act
        var result = await controller.GetTutorById(1);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var returned = Assert.IsType<Tutor>(ok.Value);
        Assert.Equal("Eduardo", returned.Nome);
    }

    [Fact]
    public async Task GetTutorById_ShouldReturnNotFound_WhenTutorDoesNotExist()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create(nameof(GetTutorById_ShouldReturnNotFound_WhenTutorDoesNotExist));
        var controller = new TutoresController(context);

        // Act
        var result = await controller.GetTutorById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostTutor_ShouldCreateTutor()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create(nameof(PostTutor_ShouldCreateTutor));
        var controller = new TutoresController(context);
        var tutor = new Tutor { Nome = "Ana", Telefone = "11888888888", Email = "ana@email.com" };

        // Act
        var result = await controller.PostTutor(tutor);

        // Assert
        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(1, context.Tutores.Count());
        Assert.Equal("Ana", ((Tutor)created.Value!).Nome);
    }

    [Fact]
    public async Task DeleteTutor_ShouldReturnNoContent_WhenTutorExists()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create(nameof(DeleteTutor_ShouldReturnNoContent_WhenTutorExists));
        context.Tutores.Add(new Tutor { Id = 1, Nome = "Ana", Telefone = "11888888888", Email = "ana@email.com" });
        await context.SaveChangesAsync();
        var controller = new TutoresController(context);

        // Act
        var result = await controller.DeleteTutor(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Empty(context.Tutores);
    }
}
