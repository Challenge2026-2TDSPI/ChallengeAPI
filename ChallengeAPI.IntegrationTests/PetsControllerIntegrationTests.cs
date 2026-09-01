using System.Net;
using System.Net.Http.Json;
using ChallengeAPI.Models;
using Xunit;

namespace ChallengeAPI.IntegrationTests;

[Collection(ApiCollection.Name)]
public class PetsControllerIntegrationTests
{
    private readonly HttpClient _client;

    public PetsControllerIntegrationTests(ApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetPets_ShouldReturnOk()
    {
        // Arrange
        // Act
        var response = await _client.GetAsync("/api/Pets");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetPetById_ShouldReturnNotFound_WhenPetDoesNotExist()
    {
        // Arrange
        // Act
        var response = await _client.GetAsync("/api/Pets/99999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PostAndGetPet_ShouldPersistData()
    {
        // Arrange
        var pet = new Pet
        {
            Nome = "Thor",
            Especie = "Cachorro",
            Raca = "Golden Retriever",
            Idade = 3,
            TutorId = 1,
            ClinicaId = 1
        };

        // Act
        var postResponse = await _client.PostAsJsonAsync("/api/Pets", pet);
        var created = await postResponse.Content.ReadFromJsonAsync<Pet>();
        var getResponse = await _client.GetAsync($"/api/Pets/{created!.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
    }

    [Fact]
    public async Task PutPet_ShouldReturnNoContent()
    {
        // Arrange
        var pet = new Pet
        {
            Nome = "Luna",
            Especie = "Gato",
            Raca = "SRD",
            Idade = 2,
            TutorId = 1,
            ClinicaId = 1
        };

        var postResponse = await _client.PostAsJsonAsync("/api/Pets", pet);
        var created = await postResponse.Content.ReadFromJsonAsync<Pet>();
        created!.Nome = "Luna Atualizada";

        // Act
        var response = await _client.PutAsJsonAsync($"/api/Pets/{created.Id}", created);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}
