using System.Net;
using System.Net.Http.Json;
using ChallengeAPI.Models;
using Xunit;

namespace ChallengeAPI.IntegrationTests;

[Collection(ApiCollection.Name)]
public class CrudControllersIntegrationTests
{
    private readonly HttpClient _client;

    public CrudControllersIntegrationTests(ApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Tutor_Post_ShouldReturnCreated()
    {
        // Arrange
        var tutor = new Tutor
        {
            Nome = "Carlos",
            Telefone = "11977777777",
            Email = "carlos@email.com"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Tutores", tutor);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Vacina_Post_ShouldReturnCreated()
    {
        // Arrange
        var vacina = new Vacina
        {
            NomeVacina = "V10",
            DataAplicacao = "2026-08-25",
            ProximaDose = "2027-08-25",
            PetId = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Vacinas", vacina);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Consulta_Post_ShouldReturnCreated()
    {
        // Arrange
        var consulta = new Consulta
        {
            DataConsulta = "2026-08-25",
            Descricao = "Consulta preventiva",
            Veterinario = "Dra. Maria",
            PetId = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Consulta", consulta);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

}
